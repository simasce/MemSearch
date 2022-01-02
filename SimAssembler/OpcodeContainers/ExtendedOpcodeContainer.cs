using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimAssembler.OpcodeParameters;

namespace SimAssembler.OpcodeContainers
{
    internal class ExtendedOpcodeContainer
    {
        /// <summary>
        /// Opcodes with 0F prefix <br/>
        /// SSE/MMX/VMX extensions are skipped.
        /// </summary>
        public static readonly List<Opcode> Opcodes_0F = new List<Opcode>()
        {
            new OpcodeExtended(new string[] { "SLDT", "STR", "LLDT", "LTR", "VERR", "VERW" })
            {
                Opbyte = 0x00,
                Name = "00MULTI",
                Parameters = new List<OpcodeParameter>() 
                {
                    new Register(RegisterType.R16, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x02,
                Name = "LAR",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R16, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x03,
                Name = "LSL",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R16, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x06,
                Name = "CLTS",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x07,
                Name = "INVD",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x09,
                Name = "WBINVD",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x0B,
                Name = "UD2",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x0D,
                Name = "NOP",
                Parameters = new List<OpcodeParameter>() 
                { 
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x18,
                Name = "NOP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x19,
                Name = "NOP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x1A,
                Name = "NOP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x1B,
                Name = "NOP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x1C,
                Name = "NOP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x1D,
                Name = "NOP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x1E,
                Name = "NOP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x1F,
                Name = "NOP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x40,
                Name = "CMOVO",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x41,
                Name = "CMOVNO",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x42,
                Name = "CMOVB",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x43,
                Name = "CMOVNB",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x44,
                Name = "CMOVZ",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x45,
                Name = "CMOVNZ",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x46,
                Name = "CMOVBE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x47,
                Name = "CMOVNBE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x48,
                Name = "CMOVS",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x49,
                Name = "CMOVNS",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x4A,
                Name = "CMOVP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x4B,
                Name = "CMOVNP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x4C,
                Name = "CMOVL",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x4D,
                Name = "CMOVNL",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x4E,
                Name = "CMOVLE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x4F,
                Name = "CMOVNLE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x80,
                Name = "JO",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x81,
                Name = "JNO",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x82,
                Name = "JB",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x83,
                Name = "JAE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x84,
                Name = "JE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x85,
                Name = "JNE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x86,
                Name = "JBE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x87,
                Name = "JA",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x88,
                Name = "JS",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x89,
                Name = "JNS",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x8A,
                Name = "JP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x8B,
                Name = "JNP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x8C,
                Name = "JL",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x8D,
                Name = "JGE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x8E,
                Name = "JLE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x8F,
                Name = "JG",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x90,
                Name = "SETO",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x91,
                Name = "SETNO",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x92,
                Name = "SETC",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x93,
                Name = "SETNC",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x94,
                Name = "SETZ",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x95,
                Name = "SETNZ",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x96,
                Name = "SETBE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x97,
                Name = "SETNBE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x98,
                Name = "SETS",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x99,
                Name = "SETNS",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x9A,
                Name = "SETP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x9B,
                Name = "SETNP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x9C,
                Name = "SETL",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x9D,
                Name = "SETNL",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x9E,
                Name = "SETLE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0x9F,
                Name = "SETNLE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0xA0,
                Name = "PUSH FS",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xA1,
                Name = "POP FS",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xA2,
                Name = "CPUID",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xA3,
                Name = "BT",
                Parameters = new List<OpcodeParameter>() 
                { 
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG)
                }
            },
            new Opcode()
            {
                Opbyte = 0xA4,
                Name = "SHLD",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xA5,
                Name = "SHLD",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                    new Static("CL")
                }
            },
            new Opcode()
            {
                Opbyte = 0xA8,
                Name = "PUSH GS",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xA9,
                Name = "POP GS",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xAA,
                Name = "RSM",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xAB,
                Name = "BTS",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0xAC,
                Name = "SHRD",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xAD,
                Name = "SHRD",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                    new Static("CL")
                }
            },
            new Opcode()
            {
                Opbyte = 0xAF,
                Name = "IMUL",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0xB0,
                Name = "CMPXCHG",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0xB1,
                Name = "CMPXCHG",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0xB2,
                Name = "LSS",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0xB3,
                Name = "BTR",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0xB4,
                Name = "LFS",
                Parameters = new List<OpcodeParameter>()
                {
                     new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0xB5,
                Name = "LGS",
                Parameters = new List<OpcodeParameter>()
                {
                     new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0xB6,
                Name = "MOVZX",
                Parameters = new List<OpcodeParameter>()
                {
                     new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R8, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0xB7,
                Name = "MOVZX",
                Parameters = new List<OpcodeParameter>()
                {
                     new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R16, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0xB9,
                Name = "UD1",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xBB,
                Name = "BTC",
                Parameters = new List<OpcodeParameter>() 
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0xBC,
                Name = "BSF",
                Parameters = new List<OpcodeParameter>()
                {
                     new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0xBD,
                Name = "BSR",
                Parameters = new List<OpcodeParameter>()
                {
                     new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0xBE,
                Name = "MOVSX",
                Parameters = new List<OpcodeParameter>()
                {
                     new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R8, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0xBF,
                Name = "MOVSX",
                Parameters = new List<OpcodeParameter>()
                {
                     new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R16, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0xC0,
                Name = "XADD",
                Parameters = new List<OpcodeParameter>()
                {
                     new Register(RegisterType.R8, RegisterDirection.RM, RegisterType.R8, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0xC1,
                Name = "XADD",
                Parameters = new List<OpcodeParameter>()
                {
                     new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
        };


        /// <summary>
        /// Opcodes with F3 prefix
        /// </summary>
        public static readonly List<Opcode> Opcodes_F3 = new List<Opcode>()
        {
            new Opcode()
            {
                Opbyte = 0x90,
                Name = "PAUSE",
                Parameters = new List<OpcodeParameter>() { }
            }
        };
    }
}
