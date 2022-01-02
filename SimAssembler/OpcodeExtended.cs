using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler
{
    internal class OpcodeExtended : Opcode
    {
        public string[] OpcodeNames { get; private set; }
        public OpcodeExtended(string[] opcodeNames)
        {
            if (opcodeNames.Length < 7)
                Array.Resize(ref opcodeNames, 7);

            OpcodeNames = opcodeNames;           
        }

        public override string GetName(MemoryStream ms)
        {
            byte regInfo = (byte)ms.ReadByte();
            byte reg = (byte)(((int)regInfo & 0x38) >> 3);   //0x38 - 00111000 bit
            ms.Seek(-1, SeekOrigin.Current);

            return OpcodeNames[reg];
        }
    }
}
