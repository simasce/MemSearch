using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler
{  
    //64 bit registers use special bit to see which ones to use, check bottom of coderx86 when implementing later!
    internal enum RegisterType
    {
        R8,
        R16,
        R32,
        R64,
        MM,
        XMM,
        SREG,

        NONE
    }

    internal enum RegisterDirection
    {
        REG,
        RM,
        NONE
    }

    internal class RegisterInfo
    {
        public int Index;
        public RegisterType Type;
    }

    internal static class RegisterTools
    {
        public static readonly string[] R8_Registers = { "AL", "CL", "DL", "BL", "AH", "CH", "DH", "BH" };
        public static readonly string[] R16_Registers = { "AX", "CX", "DX", "BX", "SP", "BP", "SI", "DI" };
        public static readonly string[] R32_Registers = { "EAX", "ECX", "EDX", "EBX", "ESP", "EBP", "ESI", "EDI" };
        public static readonly string[] MM_Registers = { "MM0", "MM1", "MM2", "MM3", "MM4", "MM5", "MM6", "MM7" };
        public static readonly string[] XMM_Registers = { "XMM0", "XMM1", "XMM2", "XMM3", "XMM4", "XMM5", "XMM6", "XMM7" };
        public static readonly string[] SREG_Registers = { "ES", "CS", "SS", "DS", "FS", "GS", "", "" };

        public static readonly RegisterType Arch_Type = RegisterType.R32; // Note: 64-bit assembly CAN take 32 bit address references, this sort of RM fixup in Register.cs won't work.

        private static readonly List<string[]> Register_Levels = new List<string[]>()
        {
            R8_Registers,
            R16_Registers,
            R32_Registers,
            MM_Registers,
            XMM_Registers,
            SREG_Registers
        };

        public static RegisterInfo ParseRegister(string name)
        {
            RegisterInfo output = new RegisterInfo();

            if (ParsingFunction(R8_Registers, RegisterType.R8, name, output)
            || ParsingFunction(R16_Registers, RegisterType.R16, name, output)
            || ParsingFunction(R32_Registers, RegisterType.R32, name, output)
            || ParsingFunction(MM_Registers, RegisterType.MM, name, output)
            || ParsingFunction(XMM_Registers, RegisterType.XMM, name, output)
            || ParsingFunction(SREG_Registers, RegisterType.XMM, name, output))
                return output;

            return null;
        }
       
        private static bool ParsingFunction(string[] registerList, RegisterType type, string name, RegisterInfo output)
        {
            for (int i = 0; i < registerList.Length; i++)
            {
                if (registerList[i] == name)
                {
                    output.Type = type;
                    output.Index = i;
                    return true;
                }
            }
            return false;
        }

        public static string[] GetRegisterStringsForType(RegisterType type)
        {
            switch(type)
            {
                case RegisterType.R8:
                    return R8_Registers;
                case RegisterType.R16:
                    return R16_Registers;
                case RegisterType.R32:
                    return R32_Registers;
                case RegisterType.MM:
                    return MM_Registers;
                case RegisterType.XMM:
                    return XMM_Registers;
                case RegisterType.SREG:
                    return SREG_Registers;
            }
            throw new NotImplementedException();
        }

        public static string GetRegisterString(RegisterType type, int index)
        {
            return GetRegisterStringsForType(type)[index];
        }

        public static int GetRegisterTypeSize(RegisterType type)
        {
            switch (type)
            {
                case RegisterType.R8:
                    return 1;
                case RegisterType.R16:
                    return 2;
                case RegisterType.R32:
                    return 4;
                case RegisterType.R64:
                    return 8;
                case RegisterType.MM:
                    return 8;
                case RegisterType.XMM:
                    return 16;
                case RegisterType.SREG:
                    return 2;
                case RegisterType.NONE:
                    return 99;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Steps register size up or down and returns it as string
        /// </summary>
        /// <param name="index">Index of the register as given in RegisterInfo</param>
        /// <param name="type">Register type as in RegisterInfo</param>
        /// <param name="stepLevel">Positive number - higher bit register, Negative - lower bit register, Zero - do nothing</param>
        /// <returns></returns>
        public static string StepRegisterString(int index, RegisterType type, int stepLevel)
        {
            if (stepLevel == 0)
                return GetRegisterString(type, index);

            int regType = (int)type;
            int diff = regType + stepLevel;

            if (diff < 0 || diff > (int)RegisterType.NONE - 1)
                throw new ArgumentException();

            return GetRegisterString((RegisterType)diff, index);
        }

        /// <summary>
        /// Steps register size up or down
        /// </summary>
        /// <param name="index">Index of the register as given in RegisterInfo</param>
        /// <param name="type">Register type as in RegisterInfo</param>
        /// <param name="stepLevel">Positive number - higher bit register, Negative - lower bit register, Zero - do nothing</param>
        /// <returns></returns>
        public static RegisterInfo StepRegister(int index, RegisterType type, int stepLevel)
        {
            if (stepLevel == 0)
                return new RegisterInfo { Type = type, Index= index };

            int regType = (int)type;
            int diff = regType + stepLevel;

            if (diff < 0 || diff > (int)RegisterType.NONE - 1)
                throw new ArgumentException();

            return new RegisterInfo { Type = (RegisterType)diff, Index = index };
        }
    }
}
