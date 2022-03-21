using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using SimAssembler.OpcodeContainers;

namespace SimAssembler
{
    public static class Assembler
    {
        public static List<OpcodeReturnInfo> Disassemble(byte[] bytecode, out bool finished, UInt64 baseAddress=0)
        {
            List<OpcodeReturnInfo> retInfo = new List<OpcodeReturnInfo>();
            try
            {
                using (MemoryStream ms = new MemoryStream(bytecode))
                {
                    while (ms.Position < ms.Length)
                    {
                        ulong currentOffset = baseAddress + (ulong)ms.Position;

                        List<byte> opBytes = new List<byte>();

                        byte curByte = (byte)ms.ReadByte();
                        opBytes.Add(curByte);

                        byte? extrabyte = null;
                        byte? opcodeExtension = null;

                        if (curByte == 0x0F || curByte == 0xF3)
                        {
                            opcodeExtension = curByte;
                            curByte = (byte)ms.ReadByte();
                            opBytes.Add(curByte);
                        }

                        if (curByte == 0x66) // lower prefix
                        {
                            extrabyte = curByte;
                            curByte = (byte)ms.ReadByte();
                            opBytes.Add(curByte);
                        }
                        //NOTE: there is also 0x67, not sure how it's used though

                        Opcode op;

                        if (opcodeExtension == 0x0F)
                            op = ExtendedOpcodeContainer.Opcodes_0F.Where(x => x.Opbyte == curByte).FirstOrDefault();
                        else if (opcodeExtension == 0xF3)
                            op = ExtendedOpcodeContainer.Opcodes_F3.Where(x => x.Opbyte == curByte).FirstOrDefault();
                        else
                            op = OpcodeContainer.Opcodes[curByte];

                        if (op == null)
                            throw new NotImplementedException();

                        string result = op.GetName(ms);
                        var parameters = op.GetParameters(ms);
                        for (int i = 0; i < parameters.Count; i++)
                        {
                            OpcodeReadInfo info = new OpcodeReadInfo()
                            {
                                Offset = baseAddress + (ulong)ms.Position,
                                Stream = ms,
                                OverrideByte = extrabyte.HasValue ? extrabyte.Value : (byte)0x00,
                            };

                            OpcodeReturnInfo ri = parameters[i].Read(info);
                            result += " " + ri.Result;
                            if (i + 1 < parameters.Count)
                                result += ",";

                            opBytes.AddRange(ri.Bytes);
                        }

                        retInfo.Add(new OpcodeReturnInfo()
                        {
                            Bytes = opBytes,
                            Offset = currentOffset,
                            Result = result
                        });
                    }
                }

                finished = true;
                return retInfo;
            }
            catch (Exception e)
            {
                finished = false;
                return retInfo;
            }
        }

        public static List<OpcodeReturnInfo> Assemble(string assemblerText, out bool finished, UInt64 baseAddress = 0)
        {
            AssemblerCompiler compiler = new AssemblerCompiler(baseAddress);

            string errorCode = "";
            if(!compiler.Compile(assemblerText, out errorCode))
            {
                //Console.WriteLine(errorCode);
                finished = false;
                return null;
            }

            finished = true;
            return compiler.Opcodes;
        }
    }
}
