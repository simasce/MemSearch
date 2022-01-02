using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler
{
    internal class OpcodeExtendedParams : OpcodeExtended
    {
        public List<OpcodeParameter>[]  OpcodeParameterArray { get; private set; }

        public OpcodeExtendedParams(string[] opcodeNames, List<OpcodeParameter>[] opcodeParamArray) : base(opcodeNames)
        {
            OpcodeParameterArray = opcodeParamArray;
        }

        public override List<OpcodeParameter> GetParameters(MemoryStream ms)
        {
            byte regInfo = (byte)ms.ReadByte();
            byte reg = (byte)(((int)regInfo & 0x38) >> 3);   //0x38 - 00111000 bit
            ms.Seek(-1, SeekOrigin.Current);

            return OpcodeParameterArray[reg];
        }
    }
}
