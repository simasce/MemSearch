using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimAssembler.OpcodeParameters;

namespace SimAssembler.OpcodeContainers
{
    internal static class OpcodeContainer
    {
        public static readonly List<Opcode> Opcodes = new List<Opcode>()
        {
            new Opcode()
            {
                Opbyte = 0x00,
                Name = "ADD",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R8, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x01,
                Name = "ADD",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x02,
                Name = "ADD",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x03,
                Name = "ADD",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x04,
                Name = "ADD",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("AL"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x05,
                Name = "ADD",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x06,
                Name = "PUSH",
                Parameters = new List<OpcodeParameter>()
                {
                    new Static("ES"),
                }
            },
            new Opcode()
            {
                Opbyte = 0x07,
                Name = "POP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Static("ES"),
                }
            },
            new Opcode()
            {
                Opbyte = 0x08,
                Name = "OR",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R8, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x09,
                Name = "OR",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x0A,
                Name = "OR",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x0B,
                Name = "OR",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x0C,
                Name = "OR",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("AL"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x0D,
                Name = "OR",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x0E,
                Name = "PUSH",
                Parameters = new List<OpcodeParameter>()
                {
                    new Static("CS"),
                }
            },
            new Opcode()
            {
                Opbyte = 0x0F,
                Name = "TWOBYTE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Invalid()
                }
            },
            new Opcode()
            {
                Opbyte = 0x10,
                Name = "ADC",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R8, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x11,
                Name = "ADC",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x12,
                Name = "ADC",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x13,
                Name = "ADC",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x14,
                Name = "ADC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("AL"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x15,
                Name = "ADC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x16,
                Name = "PUSH",
                Parameters = new List<OpcodeParameter>()
                {
                    new Static("SS"),
                }
            },
            new Opcode()
            {
                Opbyte = 0x17,
                Name = "POP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Static("SS"),
                }
            },
            new Opcode()
            {
                Opbyte = 0x18,
                Name = "SBB",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R8, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x19,
                Name = "SBB",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x1A,
                Name = "SBB",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x1B,
                Name = "SBB",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x1C,
                Name = "SBB",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("AL"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x1D,
                Name = "SBB",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x1E,
                Name = "PUSH",
                Parameters = new List<OpcodeParameter>()
                {
                    new Static("DS"),
                }
            },
            new Opcode()
            {
                Opbyte = 0x1F,
                Name = "POP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Static("DS"),
                }
            },
            new Opcode()
            {
                Opbyte = 0x20,
                Name = "AND",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R8, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x21,
                Name = "AND",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x22,
                Name = "AND",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x23,
                Name = "AND",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x24,
                Name = "AND",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("AL"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x25,
                Name = "AND",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x26,
                Name = "ES",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x27,
                Name = "DAA",
                Parameters = new List<OpcodeParameter>()
                {
                    new Static("AL"),
                }
            },
            new Opcode()
            {
                Opbyte = 0x28,
                Name = "SUB",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R8, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x29,
                Name = "SUB",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x2A,
                Name = "SUB",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x2B,
                Name = "SUB",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x2C,
                Name = "SUB",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("AL"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x2D,
                Name = "SUB",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x2E,
                Name = "CS",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x2F,
                Name = "DAS",
                Parameters = new List<OpcodeParameter>()
                {
                    new Static("AL"),
                }
            },
            new Opcode()
            {
                Opbyte = 0x30,
                Name = "XOR",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R8, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x31,
                Name = "XOR",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x32,
                Name = "XOR",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x33,
                Name = "XOR",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x34,
                Name = "XOR",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("AL"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x35,
                Name = "XOR",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x36,
                Name = "SS",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x37,
                Name = "AAA",
                Parameters = new List<OpcodeParameter>()
                {
                    new Static("AL"),
                    new Static("AH"),
                }
            },
            new Opcode()
            {
                Opbyte = 0x38,
                Name = "CMP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R8, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x39,
                Name = "CMP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x3A,
                Name = "CMP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x3B,
                Name = "CMP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x3C,
                Name = "CMP",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("AL"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x3D,
                Name = "CMP",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x3E,
                Name = "DS",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x3F,
                Name = "AAS",
                Parameters = new List<OpcodeParameter>()
                {
                    new Static("AL"),
                    new Static("AH"),
                }
            },
            new Opcode()
            {
                Opbyte = 0x40,
                Name = "INC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x41,
                Name = "INC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ECX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x42,
                Name = "INC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EDX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x43,
                Name = "INC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EBX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x44,
                Name = "INC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ESP")
                }
            },
            new Opcode()
            {
                Opbyte = 0x45,
                Name = "INC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EBP")
                }
            },
            new Opcode()
            {
                Opbyte = 0x46,
                Name = "INC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ESI")
                }
            },
            new Opcode()
            {
                Opbyte = 0x47,
                Name = "INC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EDI")
                }
            },
            new Opcode()
            {
                Opbyte = 0x48,
                Name = "DEC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x49,
                Name = "DEC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ECX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x4A,
                Name = "DEC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EDX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x4B,
                Name = "DEC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EBX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x4C,
                Name = "DEC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ESP")
                }
            },
            new Opcode()
            {
                Opbyte = 0x4D,
                Name = "DEC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EBP")
                }
            },
            new Opcode()
            {
                Opbyte = 0x4E,
                Name = "DEC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ESI")
                }
            },
            new Opcode()
            {
                Opbyte = 0x4F,
                Name = "DEC",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EDI")
                }
            },
            new Opcode()
            {
                Opbyte = 0x50,
                Name = "PUSH",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x51,
                Name = "PUSH",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ECX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x52,
                Name = "PUSH",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EDX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x53,
                Name = "PUSH",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EBX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x54,
                Name = "PUSH",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ESP")
                }
            },
            new Opcode()
            {
                Opbyte = 0x55,
                Name = "PUSH",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EBP")
                }
            },
            new Opcode()
            {
                Opbyte = 0x56,
                Name = "PUSH",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ESI")
                }
            },
            new Opcode()
            {
                Opbyte = 0x57,
                Name = "PUSH",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EDI")
                }
            },
            new Opcode()
            {
                Opbyte = 0x58,
                Name = "POP",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x59,
                Name = "POP",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ECX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x5A,
                Name = "POP",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EDX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x5B,
                Name = "POP",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EBX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x5C,
                Name = "POP",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ESP")
                }
            },
            new Opcode()
            {
                Opbyte = 0x5D,
                Name = "POP",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EBP")
                }
            },
            new Opcode()
            {
                Opbyte = 0x5E,
                Name = "POP",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ESI")
                }
            },
            new Opcode()
            {
                Opbyte = 0x5F,
                Name = "POP",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EDI")
                }
            },
            new Opcode()
            {
                Opbyte = 0x60,
                Name = "PUSHA",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x61,
                Name = "POPA",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x62,
                Name = "UNKN",
                Parameters = new List<OpcodeParameter>() { new Invalid() }
            },
            new Opcode()
            {
                Opbyte = 0x63,
                Name = "UNKN",
                Parameters = new List<OpcodeParameter>() { new Invalid() }
            },
            new Opcode()
            {
                Opbyte = 0x64,
                Name = "FS",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x65,
                Name = "GS",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x66,
                Name = "OPERANDSIZE",
                Parameters = new List<OpcodeParameter>() { new Invalid() }
            },
            new Opcode()
            {
                Opbyte = 0x67,
                Name = "ADDRESSSIZE",
                Parameters = new List<OpcodeParameter>() { new Invalid() }
            },
            new Opcode()
            {
                Opbyte = 0x68,
                Name = "PUSH",
                Parameters = new List<OpcodeParameter>()
                {
                    new Immediate(4)
                }                        
            },
            new Opcode()
            {
                Opbyte = 0x69,
                Name = "IMUL",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x6A,
                Name = "PUSH",
                Parameters = new List<OpcodeParameter>()
                {
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x6B,
                Name = "IMUL",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x6C,
                Name = "INS BYTE PTR ES:[EDI], DX",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x6D,
                Name = "INS DWORD PTR ES:[EDI], DX",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x6E,
                Name = "OUTS DX, BYTE PTR DS:[ESI]",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x6F,
                Name = "OUTS DX, DWORD PTR DS:[ESI]",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x70,
                Name = "JO",
                Parameters = new List<OpcodeParameter>() 
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x71,
                Name = "JNO",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x72,
                Name = "JB",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x73,
                Name = "JAE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x74,
                Name = "JZ",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x75,
                Name = "JNZ",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x76,
                Name = "JBE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x77,
                Name = "JA",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x78,
                Name = "JS",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x79,
                Name = "JNS",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x7A,
                Name = "JP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x7B,
                Name = "JNP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x7C,
                Name = "JL",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x7D,
                Name = "JGE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x7E,
                Name = "JLE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x7F,
                Name = "JG",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new OpcodeExtended(new string[] {"ADD", "OR", "ADC", "SBB", "AND", "SUB", "XOR", "CMP"})
            {
                Opbyte = 0x80,
                Name = "80MULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM),
                    new Immediate(1)
                }
            },
            new OpcodeExtended(new string[] {"ADD", "OR", "ADC", "SBB", "AND", "SUB", "XOR", "CMP"})
            {
                Opbyte = 0x81,
                Name = "81MULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM),
                    new Immediate(4)
                }
            },
            new OpcodeExtended(new string[] {"ADD", "OR", "ADC", "SBB", "AND", "SUB", "XOR", "CMP"})
            {
                Opbyte = 0x82,
                Name = "82MULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM),
                    new Immediate(1)
                }
            },
            new OpcodeExtended(new string[] {"ADD", "OR", "ADC", "SBB", "AND", "SUB", "XOR", "CMP"})
            {
                Opbyte = 0x83,
                Name = "83MULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0x84,
                Name = "TEST",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM, RegisterType.R8, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x85,
                Name = "TEST",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x86,
                Name = "XCHG",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.REG, RegisterType.R8, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x87,
                Name = "XCHG",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x88,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM, RegisterType.R8, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x89,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.R32, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x8A,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.REG, RegisterType.R8, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x8B,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x8C,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM, RegisterType.SREG, RegisterDirection.REG),
                }
            },
            new Opcode()
            {
                Opbyte = 0x8D,
                Name = "LEA",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x8E,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.SREG, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x8F,
                Name = "POP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0x90,
                Name = "NOP",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x91,
                Name = "XCHG",
                Parameters = new List<OpcodeParameter>() 
                { 
                    new StaticRegister("ECX"),
                    new StaticRegister("EAX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x92,
                Name = "XCHG",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EDX"),
                    new StaticRegister("EAX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x93,
                Name = "XCHG",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EBX"),
                    new StaticRegister("EAX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x94,
                Name = "XCHG",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ESP"),
                    new StaticRegister("EAX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x95,
                Name = "XCHG",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EBP"),
                    new StaticRegister("EAX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x96,
                Name = "XCHG",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ESI"),
                    new StaticRegister("EAX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x97,
                Name = "XCHG",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EDI"),
                    new StaticRegister("EAX")
                }
            },
            new Opcode()
            {
                Opbyte = 0x98,
                Name = "CWDE",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x99,
                Name = "CDQ",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x9A,
                Name = "CALLF",
                Parameters = new List<OpcodeParameter>() 
                {
                    new FarPointer(2, 4)
                }
            },
            new Opcode()
            {
                Opbyte = 0x9B,
                Name = "WAIT",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x9C,
                Name = "PUSHF",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x9D,
                Name = "POPF",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x9E,
                Name = "SAHF",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0x9F,
                Name = "LAHF",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xA0,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>() 
                { 
                    new StaticRegister("AL"),
                    new ImmediateDS(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0xA1,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX"),
                    new ImmediateDS(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0xA2,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new ImmediateDS(4),
                    new StaticRegister("AL")                  
                }
            },
            new Opcode()
            {
                Opbyte = 0xA3,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new ImmediateDS(4),
                    new StaticRegister("EAX")
                }
            },
            new Opcode()
            {
                Opbyte = 0xA4,
                Name = "MOVS BYTE PTR ES:[EDI], BYTE PTR DS:[ESI]",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xA5,
                Name = "MOVS DWORD PTR ES:[EDI], DWORD PTR DS:[ESI]",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xA6,
                Name = "CMPS BYTE PTR DS:[ESI], BYTE PTR ES:[EDI]",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xA7,
                Name = "CMPS DWORD PTR DS:[ESI], DWORD PTR ES:[EDI]",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xA8,
                Name = "TEST",
                Parameters = new List<OpcodeParameter>() 
                { 
                    new StaticRegister("AL"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xA9,
                Name = "TEST",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0xAA,
                Name = "STOS BYTE PTR ES:[EDI], AL",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xAB,
                Name = "STOS DWORD PTR ES:[EDI], EAX",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xAC,
                Name = "LODS AL, BYTE PTR DS:[ESI]",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xAD,
                Name = "LODS EAX, DWORD PTR DS:[ESI]",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xAE,
                Name = "SCAS AL, BYTE PTR ES:[EDI]",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xAF,
                Name = "SCAS EAX, DWORD PTR ES:[EDI]",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xB0,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>() 
                { 
                    new StaticRegister("AL"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xB1,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("CL"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xB2,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("DL"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xB3,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("BL"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xB4,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("AH"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xB5,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("CH"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xB6,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("DH"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xB7,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("BH"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xB8,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0xB9,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ECX"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0xBA,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EDX"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0xBB,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EBX"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0xBC,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ESP"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0xBD,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EBP"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0xBE,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("ESI"),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0xBF,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EDI"),
                    new Immediate(4)
                }
            },
            new OpcodeExtended(new string[] { "ROL", "ROR", "RCL", "RCR", "SHL", "SHR", "SHL", "SAR"})
            {
                Opbyte = 0xC0,
                Name = "C0MULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM),
                    new Immediate(1)
                }
            },
            new OpcodeExtended(new string[] { "ROL", "ROR", "RCL", "RCR", "SHL", "SHR", "SHL", "SAR"})
            {
                Opbyte = 0xC1,
                Name = "C1MULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xC2,
                Name = "RETN",
                Parameters = new List<OpcodeParameter>()
                {
                    new Immediate(2)
                }
            },
            new Opcode()
            {
                Opbyte = 0xC3,
                Name = "RETN",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xC4,
                Name = "LES",
                Parameters = new List<OpcodeParameter>() 
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0xC5,
                Name = "LDS",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.REG, RegisterType.R32, RegisterDirection.RM)
                }
            },
            new Opcode()
            {
                Opbyte = 0xC6,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xC7,
                Name = "MOV",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM),
                    new Immediate(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0xC8,
                Name = "ENTER",
                Parameters = new List<OpcodeParameter>()
                {
                    new Immediate(2),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xC9,
                Name = "LEAVE",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xCA,
                Name = "RETF",
                Parameters = new List<OpcodeParameter>()
                {
                    new Immediate(2),
                }
            },
            new Opcode()
            {
                Opbyte = 0xCB,
                Name = "RETF",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xCC,
                Name = "INT3",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xCD,
                Name = "INT",
                Parameters = new List<OpcodeParameter>()
                {
                    new Immediate(1),
                }
            },
            new Opcode()
            {
                Opbyte = 0xCE,
                Name = "INTO",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xCF,
                Name = "IRET",
                Parameters = new List<OpcodeParameter>() { }
            },
            new OpcodeExtended(new string[] { "ROL", "ROR", "RCL", "RCR", "SHL", "SHR", "SHL", "SAR"})
            {
                Opbyte = 0xD0,
                Name = "D0MULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM),
                    new Static("1")
                }
            },
            new OpcodeExtended(new string[] { "ROL", "ROR", "RCL", "RCR", "SHL", "SHR", "SHL", "SAR"})
            {
                Opbyte = 0xD1,
                Name = "D1MULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM),
                    new Static("1")
                }
            },
            new OpcodeExtended(new string[] { "ROL", "ROR", "RCL", "RCR", "SHL", "SHR", "SHL", "SAR"})
            {
                Opbyte = 0xD2,
                Name = "D2MULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R8, RegisterDirection.RM),
                    new Static("CL")
                }
            },
            new OpcodeExtended(new string[] { "ROL", "ROR", "RCL", "RCR", "SHL", "SHR", "SHL", "SAR"})
            {
                Opbyte = 0xD3,
                Name = "D2MULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM),
                    new Static("CL")
                }
            },
            new Opcode()
            {
                Opbyte = 0xD4,
                Name = "AAM",
                Parameters = new List<OpcodeParameter>() 
                { 
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xD5,
                Name = "AAD",
                Parameters = new List<OpcodeParameter>()
                {
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xD6,
                Name = "UNDEFINED",
                Parameters = new List<OpcodeParameter>() { new Invalid() }              
            },
            new Opcode()
            {
                Opbyte = 0xD7,
                Name = "XLAT BYTE PTR DS:[EBX]",
                Parameters = new List<OpcodeParameter>() { }
            },
            new OpcodeExtended(new string[] { "FADD", "FMUL", "FCOM", "FCOMP", "FSUB", "FSUBR", "FDIV", "FDIVR"})
            {
                Opbyte = 0xD8,
                Name = "D8MULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM),
                }
            },
            new OpcodeExtended(new string[] { "FLD", "FXCH", "FST", "FSTP", "FABS", "FLDZ", "FSTENV", "FLDENV"})
            {
                Opbyte = 0xD9,
                Name = "D9MULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM),
                }
            },
            new OpcodeExtended(new string[] { "FIADD", "FIMUL", "FICOM", "FICOMP", "FISUB", "FISUBR", "FIDIV", "FIDIVR"})
            {
                Opbyte = 0xDA,
                Name = "DAMULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM),
                }
            },
            new OpcodeExtended(new string[] { "FCMOVNB", "FISTTP", "FIST", "FISTP", "FINIT", "FLD", "FCOMI", "FSTP"})
            {
                Opbyte = 0xDB,
                Name = "DBMULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM),
                }
            },
            new OpcodeExtended(new string[] { "FADD", "FMUL", "FCOM", "FCOMP", "FSUB", "FSUBR", "FDIV", "FDIVR"})
            {
                Opbyte = 0xDC,
                Name = "DCMULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R64, RegisterDirection.RM),
                }
            },
            new OpcodeExtended(new string[] { "FLD", "FISTTP", "FST", "FSTP", "FUCOM", "FUCOMP", "FSAVE", "FSTSW"})
            {
                Opbyte = 0xDD,
                Name = "DDMULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM),
                }
            },
            new OpcodeExtended(new string[] { "FIADD", "FIMUL", "FICOM", "FICOMP", "FISUB", "FSUBP", "FIDIV", "FIDIVP"})
            {
                Opbyte = 0xDE,
                Name = "DEMULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM),
                }
            },
            new OpcodeExtended(new string[] { "FILD", "FISTTP", "FIST", "FISTP", "FBLD", "FILD", "FBSTP", "FISTP"})
            {
                Opbyte = 0xDF,
                Name = "DFMULTI",
                Parameters = new List<OpcodeParameter>()
                {
                    new Register(RegisterType.R32, RegisterDirection.RM),
                }
            },
            new Opcode()
            {
                Opbyte = 0xE0,
                Name = "LOOPNE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xE1,
                Name = "LOOPE",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xE2,
                Name = "LOOP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xE3,
                Name = "JECXZ",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xE4,
                Name = "IN",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("AL"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xE5,
                Name = "IN",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX"),
                    new Immediate(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xE6,
                Name = "OUT",
                Parameters = new List<OpcodeParameter>()
                {   
                    new Immediate(1),
                    new StaticRegister("AL")
                }
            },
            new Opcode()
            {
                Opbyte = 0xE7,
                Name = "OUT",
                Parameters = new List<OpcodeParameter>()
                {              
                    new Immediate(1),
                    new StaticRegister("EAX")
                }
            },
            new Opcode()
            {
                Opbyte = 0xE8,
                Name = "CALL",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0xE9,
                Name = "JMP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(4)
                }
            },
            new Opcode()
            {
                Opbyte = 0xEA,
                Name = "JMPF",
                Parameters = new List<OpcodeParameter>()
                {
                    new FarPointer(2, 4)
                }
            },
            new Opcode()
            {
                Opbyte = 0xEB,
                Name = "JMP",
                Parameters = new List<OpcodeParameter>()
                {
                    new Relative(1)
                }
            },
            new Opcode()
            {
                Opbyte = 0xEC,
                Name = "IN",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("AL"),
                    new StaticRegister("DX")
                }
            },
            new Opcode()
            {
                Opbyte = 0xED,
                Name = "IN",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("EAX"),
                    new StaticRegister("DX")
                }
            },
            new Opcode()
            {
                Opbyte = 0xEE,
                Name = "OUT",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("DX"),
                    new StaticRegister("AL")                   
                }
            },
            new Opcode()
            {
                Opbyte = 0xEF,
                Name = "OUT",
                Parameters = new List<OpcodeParameter>()
                {
                    new StaticRegister("DX"),
                    new StaticRegister("EAX")
                }
            },
            new Opcode()
            {
                Opbyte = 0xF0,
                Name = "LOCK",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xF1,
                Name = "ICEBP",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xF2,
                Name = "REPNZ",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xF3,
                Name = "REPZ",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xF4,
                Name = "HLT",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xF5,
                Name = "CMC",
                Parameters = new List<OpcodeParameter>() { }
            },
            new OpcodeExtendedParams(
                new string[] {"TEST", "TEST", "NOT", "NEG", "MUL", "IMUL", "DIV", "IDIV"},
                new List<OpcodeParameter>[] 
                { 
                    new List<OpcodeParameter>() { new Register(RegisterType.R8, RegisterDirection.RM), new Immediate(1) },
                    new List<OpcodeParameter>() { new Register(RegisterType.R8, RegisterDirection.RM), new Immediate(1) },
                    new List<OpcodeParameter>() { new Register(RegisterType.R8, RegisterDirection.RM) },
                    new List<OpcodeParameter>() { new Register(RegisterType.R8, RegisterDirection.RM) },
                    new List<OpcodeParameter>() { new Register(RegisterType.R8, RegisterDirection.RM) },
                    new List<OpcodeParameter>() { new Register(RegisterType.R8, RegisterDirection.RM) },
                    new List<OpcodeParameter>() { new Register(RegisterType.R8, RegisterDirection.RM) },
                    new List<OpcodeParameter>() { new Register(RegisterType.R8, RegisterDirection.RM) }
                }
                )
            {
                Opbyte = 0xF6,
                Name = "F6MULTI",
                Parameters = new List<OpcodeParameter>() { }
            },
            new OpcodeExtendedParams(
                new string[] {"TEST", "TEST", "NOT", "NEG", "MUL", "IMUL", "DIV", "IDIV"},
                new List<OpcodeParameter>[]
                {
                    new List<OpcodeParameter>() { new Register(RegisterType.R32, RegisterDirection.RM), new Immediate(4) },
                    new List<OpcodeParameter>() { new Register(RegisterType.R32, RegisterDirection.RM), new Immediate(4) },
                    new List<OpcodeParameter>() { new Register(RegisterType.R32, RegisterDirection.RM) },
                    new List<OpcodeParameter>() { new Register(RegisterType.R32, RegisterDirection.RM) },
                    new List<OpcodeParameter>() { new Register(RegisterType.R32, RegisterDirection.RM) },
                    new List<OpcodeParameter>() { new Register(RegisterType.R32, RegisterDirection.RM) },
                    new List<OpcodeParameter>() { new Register(RegisterType.R32, RegisterDirection.RM) },
                    new List<OpcodeParameter>() { new Register(RegisterType.R32, RegisterDirection.RM) }
                }
                )
            {
                Opbyte = 0xF7,
                Name = "F7MULTI",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xF8,
                Name = "CLC",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xF9,
                Name = "STC",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xFA,
                Name = "CLI",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xFB,
                Name = "STI",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xFC,
                Name = "CLD",
                Parameters = new List<OpcodeParameter>() { }
            },
            new Opcode()
            {
                Opbyte = 0xFD,
                Name = "STD",
                Parameters = new List<OpcodeParameter>() { }
            },
            new OpcodeExtended(new string[] {"INC", "DEC"})
            {
                Opbyte = 0xFE,
                Name = "FEMULTI",
                Parameters = new List<OpcodeParameter>() 
                { 
                    new Register(RegisterType.R8, RegisterDirection.RM)
                }
            },
            new OpcodeExtended(new string[] {"INC", "DEC", "CALL", "CALLF", "JMP", "JMPF", "PUSH"} )
            {
                Opbyte = 0xFF,
                Name = "FFMULTI",
                Parameters = new List<OpcodeParameter>() 
                {
                    new Register(RegisterType.R32, RegisterDirection.RM)
                }
            },
        };        
    }
}

