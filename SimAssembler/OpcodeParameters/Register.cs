using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler.OpcodeParameters
{    
    internal class Register : OpcodeParameter
    {
        public RegisterType FirstType { get; private set; }
        public RegisterType SecondType { get; private set; }

        public RegisterDirection FirstDirection { get; private set; }
        public RegisterDirection SecondDirection { get; private set; }

        public Register(RegisterType first_type, RegisterDirection first_direction)
        {
            FirstType = first_type;
            FirstDirection = first_direction;

            SecondType = RegisterType.NONE;
            SecondDirection = RegisterDirection.NONE;
        }

        public Register(RegisterType first_type, RegisterDirection first_direction, RegisterType second_type, RegisterDirection second_direction)
        {
            FirstType = first_type;
            FirstDirection = first_direction;

            SecondType = second_type;
            SecondDirection = second_direction;
        }

        public override OpcodeReturnInfo Read(OpcodeReadInfo info)
        {
            List<byte> extraBytes = new List<byte>(); //cannot make it global because same opcode can share it 

            MemoryStream ms = info.Stream;
            byte regInfo = (byte)ms.ReadByte();

            byte mod = (byte)(((int)regInfo & 0xC0) >> 6);   //0xC0 - 11000000 bit
            byte reg = (byte)(((int)regInfo & 0x38) >> 3);   //0x38 - 00111000 bit
            byte rm  = (byte)(((int)regInfo & 0x07));        //0x07 - 00000111 bit

            string ret = "";
            
            switch (FirstDirection)
            {
                case RegisterDirection.REG:
                    ret = ParseRegister(reg, FirstType, info.OverrideByte);
                    break;
                case RegisterDirection.RM:
                    ret = ParseRM(extraBytes, ms, FirstType, mod, rm, info.OverrideByte);
                    if (ret.StartsWith('['))
                        ret = GetRMPrefix(info.OverrideByte) + " " + ret;
                    break;
            }

            switch (SecondDirection)
            {
                case RegisterDirection.REG:
                    ret += ", " + ParseRegister(reg, SecondType, info.OverrideByte);
                    break;
                case RegisterDirection.RM:
                    string rmm = ParseRM(extraBytes, ms, SecondType, mod, rm, info.OverrideByte);
                    if (rmm.StartsWith('['))
                        rmm = GetRMPrefix(info.OverrideByte) + " " + rmm;
                    ret += ", " + rmm;
                    break;
            }

            extraBytes.Insert(0, regInfo);

            return new OpcodeReturnInfo()
            {
                Result = ret,
                Bytes = extraBytes.ToArray(),
                Offset = info.Offset
            };
        }

        private string ParseRegister(byte reg, RegisterType type, byte overrideByte)
        {
            if (reg > 7)
                throw new IndexOutOfRangeException();

            if (overrideByte == 0x66)
                return RegisterTools.StepRegisterString(reg, type, -1);
            else
                return RegisterTools.GetRegisterString(type, reg);

            throw new NotSupportedException();
        }

        private string ParseRM(List<byte> extraBytes, MemoryStream ms, RegisterType type, byte mod, byte rm, byte overrideByte)
        {          
            if (mod == 0)
            {
                if (rm < 4 || rm > 5)
                    return "[" + RegisterTools.GetRegisterString(RegisterTools.Arch_Type, rm) + "]";
                else if (rm == 4)
                    return ReadSIB(extraBytes, ms, mod);
                else
                    return "[" + ReadDisp32(extraBytes, ms) + "]";
            }
            else if(mod == 1)
            {               
                if (rm != 4)
                {
                    string disp8 = ReadDisp8(extraBytes, ms);
                    return "[" + RegisterTools.GetRegisterString(RegisterTools.Arch_Type, rm) + "+" + disp8 + "]";
                }                   
                else
                {
                    string sib_ret = ReadSIB(extraBytes, ms, mod);
                    string sib_trimmed = sib_ret.Substring(1, sib_ret.Length - 2);
                    string disp8 = ReadDisp8(extraBytes, ms);
                    return "[" + sib_trimmed + "+" + disp8 + "]";
                }                   
            }
            else if(mod == 2)
            {              
                if (rm != 4)
                {
                    string disp32 = ReadDisp32(extraBytes, ms);
                    return "[" + RegisterTools.GetRegisterString(RegisterTools.Arch_Type, rm) + "+" + disp32 + "]";
                }
                else
                {
                    string sib_ret = ReadSIB(extraBytes, ms, mod);
                    string sib_trimmed = sib_ret.Substring(1, sib_ret.Length - 2);
                    string disp32 = ReadDisp32(extraBytes, ms);
                    return "[" + sib_trimmed + "+" + disp32 + "]";
                }
            }
            else if(mod == 3)
            {
                if (rm > 7)
                    throw new IndexOutOfRangeException();

                if (overrideByte == 0x66)
                    return RegisterTools.StepRegisterString(rm, type, -1);
                else
                    return RegisterTools.GetRegisterString(type, rm);
            }

            return "";
        }

        private string ReadSIB(List<byte> extraBytes, MemoryStream ms, byte mod)
        {
            byte sib = (byte)ms.ReadByte();
            extraBytes.Add(sib);

            byte _scale = (byte)(((int)sib & 0xC0) >> 6);   //0xC0 - 11000000 bit
            byte index = (byte)(((int)sib & 0x38) >> 3);   //0x38 - 00111000 bit
            byte _base = (byte)(((int)sib & 0x07));        //0x07 - 00000111 bit

            int scale = (int)Math.Pow(2, (int)_scale); // 00 - *1, 01 - *2, 10 - *3, 11 - *4
            string base_register = "";

            if (_base == 5)
            {
                if (mod == 0)
                    base_register = ReadDisp32(extraBytes, ms);
                else
                    base_register = "EBP";
            }               
            else
                base_register = RegisterTools.R32_Registers[_base];

            string index_register = RegisterTools.GetRegisterString(RegisterType.R32, index);

            if (_base == 5 && mod == 0)
                return $"[{index_register}*{scale.ToString()}+{base_register}]";

            return $"[{base_register}+{index_register}*{scale.ToString()}]";
        }

        private string ReadDisp8(List<byte> extraBytes, MemoryStream ms)
        {
            byte bb = (byte)ms.ReadByte();
            extraBytes.Add(bb);

            return "0x" + bb.ToString("X2");
        }

        private string ReadDisp32(List<byte> extraBytes, MemoryStream ms)
        {
            byte[] buffer = new byte[4];
            ms.Read(buffer, 0, 4);

            extraBytes.AddRange(buffer);

            Array.Reverse(buffer);

            return "0x" + string.Join("", buffer.Select(f => f.ToString("X2")));
        }

        private string GetRMPrefix(byte overrideByte)
        {
            int smallerSize;

            if(overrideByte == 0x66)
                smallerSize = Math.Min(RegisterTools.GetRegisterTypeSize(RegisterTools.StepRegister(0, FirstType, -1).Type),
                                       RegisterTools.GetRegisterTypeSize(RegisterTools.StepRegister(0, SecondType, -1).Type));            
            else
                smallerSize = Math.Min(RegisterTools.GetRegisterTypeSize(FirstType), RegisterTools.GetRegisterTypeSize(SecondType));

            switch (smallerSize)
            {
                case 1:
                    return "BYTE PTR";
                case 2:
                    return "WORD PTR";
                case 4:
                    return "DWORD PTR";
                case 8:
                    return "QWORD PTR";
            }
            return "";
        }
    }
}
