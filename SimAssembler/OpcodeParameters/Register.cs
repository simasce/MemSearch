using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
                Bytes = extraBytes,
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

        public override bool Compile(string fullString, ref List<byte> compiledBytes, ref List<byte> extraFrontBytes, ref List<LinkerRequestEntry> linkerRequests)
        {
            string[] parms = fullString.Replace(" ", "").Split(',').Take(2).ToArray();
            if (parms.Length == 0)
                return false;

            bool twoMods = false, stepDown = false;
            RegisterInfo info1 = null, info2 = null;

            byte outputByte = 0;
            List<byte> additionalBytes = new List<byte>();

            string[] invalidStrings = { "QWORDPTR", "DWORDPTR", "WORDPTR", "BYTEPTR" };
            foreach (string str in invalidStrings)
                parms[0] = parms[0].Replace(str, "");

            //Custom check if one parameter
            if(FirstDirection == RegisterDirection.RM && SecondDirection == RegisterDirection.NONE)
            {
                if (!parms[0].StartsWith("["))
                    outputByte |= 0xC0; //mod 11

                parms[0] = parms[0].Replace("[", "").Replace("]", "");

                byte[] regBits; //unused
                if ((parms.Length == 1 || ConversionHelper.TryConvertNumericBySize(parms[1], 4, out regBits)) && Regex.Match(parms[0], @"[A-Z]+").Success)
                {
                    RegisterInfo regInfo = null;
                    regInfo = RegisterTools.ParseRegister(parms[0]);
                    if (regInfo == null)
                        return false;

                    if(regInfo.Type != FirstType)
                    {
                        if (regInfo.Type == FirstType - 1)
                            extraFrontBytes.Add(0x66);
                        else
                            return false;
                    }

                    if(regInfo != null)
                    {
                        outputByte |= (byte)regInfo.Index;
                        compiledBytes.Add(outputByte);
                        return true;
                    }
                }
                return false;
            }

            if (parms.Length != 2)
                return false;

            if (!CheckOpcodeCompatibility(parms, ref info1, ref info2, out stepDown, out twoMods))
            {
                return false;
            }
            else
            {
                if (twoMods) //mod == 3
                {
                    outputByte |= 0xC0; //0xC0 - 11000000 bit

                    outputByte |= (byte)((FirstDirection == RegisterDirection.REG ? info1 : info2).Index << 3); //reg
                    outputByte |= (byte)((FirstDirection == RegisterDirection.RM ? info1 : info2).Index); //rm
                }
                else
                {
                    try
                    {
                        outputByte |= (byte)((FirstDirection == RegisterDirection.REG ? info1 : info2).Index << 3); //reg
                    }
                    catch(Exception e)
                    {
                        return false;
                    }

                    //process RM and MOD
                    RegisterInfo rmInfo = (FirstDirection == RegisterDirection.RM ? info1 : info2);
                    string rmParam = (FirstDirection == RegisterDirection.RM ? parms[0] : parms[1]);
                    if (rmParam.Length < 2)
                        return false;

                    string rmClean = rmParam.Substring(1, rmParam.Length - 2);
                    if (Regex.Match(rmParam, @"\[[A-Z]+\]").Success) //[Register] or [Pointer]
                    {
                        RegisterInfo regInfo = null;
                        regInfo = RegisterTools.ParseRegister(rmClean);

                        if (regInfo != null)
                        {
                            if (regInfo.Type != RegisterType.R32)
                                return false;

                            if (regInfo.Index == 5) //ebp needs sib
                            {
                                outputByte |= 0x45; //01000101 mod = 1, rm = 101 (5)
                                additionalBytes.Add(0x00); //EBP sib
                            }
                            else
                            {
                                if (regInfo.Index != 4)
                                    outputByte |= (byte)regInfo.Index;
                                else
                                    return false;
                            }
                        }
                        else
                        {
                            outputByte |= 5;
                            additionalBytes.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                            linkerRequests.Add(new LinkerRequestEntry()
                            {
                                Relative = false,
                                Offset = compiledBytes.Count + extraFrontBytes.Count + 1 + (stepDown ? 1 : 0),
                                Size = 4,
                                PointerName = rmClean
                            });
                        }
                    }
                    else if (Regex.Match(rmParam, @"\[(0X[0-9]+|[0-9]+)\]").Success) //[123] or [0X123]
                    {
                        byte[] compiled;
                        if (ConversionHelper.TryConvertNumericBySize(rmClean, 4, out compiled))
                        {
                            outputByte |= 5;
                            additionalBytes.AddRange(compiled);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (Regex.Match(rmParam, @"\[[A-Z]+\+(0X[0-9]+|[0-9]+)\]").Success) //[Register+123] or [Register+0X123] (disp32/disp8)
                    {
                        string[] rez = rmClean.Split('+');
                        if (rez.Length != 2)
                            return false;

                        RegisterInfo regInfo = null;
                        regInfo = RegisterTools.ParseRegister(rez[0]);

                        if (regInfo == null)
                            return false;

                        if (regInfo.Index == 4)
                            return false;

                        byte[] compiled;
                        if (ConversionHelper.TryConvertNumericBySize(rez[1], 1, out compiled))
                        {
                            outputByte |= 0x40; // mod 1
                            outputByte |= (byte)regInfo.Index;
                            additionalBytes.AddRange(compiled);
                        }
                        else if (ConversionHelper.TryConvertNumericBySize(rez[1], 4, out compiled))
                        {
                            outputByte |= 0x80; // mod 2
                            outputByte |= (byte)regInfo.Index;
                            additionalBytes.AddRange(compiled);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (Regex.Match(rmParam, @"\[[A-Z]+\*(0X[0-9]+|[0-9]+)\+(0X[0-9]+|[0-9]+)\]").Success) //[Register*(0-8)+Disp32]
                    {
                        /* string[] rez = rmClean.Split('*');
                         if (rez.Length != 2)
                             return false;

                         string[] rezz = rez[1].Split('+');
                         if (rezz.Length != 2)
                             return false;

                         byte[] compiledScale, compiledDisplacement;
                         if (!ConversionHelper.TryConvertNumericBySize(rezz[0], 1, out compiledScale))
                             return false;

                         if (!ConversionHelper.TryConvertNumericBySize(rezz[1], 4, out compiledDisplacement))
                             return false;

                         RegisterInfo regInfo = null;
                         try
                         {
                             regInfo = RegisterTools.ParseRegister(rez[0]);
                         }
                         catch (NotImplementedException e) { }

                         if (regInfo == null)
                             return false;

                         byte sib = 0;
                         sib |= (byte)(compiledScale[0] << (byte)6);*/

                        return false;

                    }
                    else if (Regex.Match(rmParam, @"\[[A-Z]+\+[A-Z]+\*(0X[0-9]+|[0-9]+)\]").Success) //[Register+Register*(0-8)]
                    {
                        return false;
                    }
                    else if (Regex.Match(rmParam, @"\[[A-Z]+\+[A-Z]+\*(0X[0-9]+|[0-9]+)\+(0X[0-9]+|[0-9]+)\]").Success) //[Register+Register*(0-8)+disp32/disp8]
                    {
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }                     

            if (stepDown)
                extraFrontBytes.Add(0x66); // step down byte

            compiledBytes.Add(outputByte);
            compiledBytes.AddRange(additionalBytes);
            return true;
        }

        private bool CheckOpcodeCompatibility(string[] parms, ref RegisterInfo info1, ref RegisterInfo info2, out bool stepDown, out bool twoMods)
        {
            stepDown = false;
            twoMods = false;

            info1 = RegisterTools.ParseRegister(parms[0]);
            info2 = RegisterTools.ParseRegister(parms[1]);

            if (info1 == null && info2 == null)
                return false;

            if (info1 == null && info2 != null)
            {
                if (info2.Type != SecondType)
                {
                    if (info2.Type != SecondType - 1)
                        return false;
                    else
                        stepDown = true;
                }
            }
            else if (info1 != null && info2 == null)
            {
                if (info1.Type != FirstType)
                {
                    if (info1.Type != FirstType - 1)
                        return false;
                    else
                        stepDown = true;
                }
            }
            else
            {
                twoMods = true;
                if (info1.Type != FirstType || info2.Type != SecondType)
                {
                    if (info1.Type != FirstType - 1 && info2.Type != SecondType - 1)
                        return false;
                    else
                        stepDown = true;
                }
            }

            return true;
        }
    }
}
