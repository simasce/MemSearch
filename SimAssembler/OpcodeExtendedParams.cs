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

        public override bool SuitableOpcode(string name, int numParameters)
        {
            for(int i = 0; i < OpcodeNames.Length; i++)
            {
                string names = OpcodeNames[i];

                if (names == null)
                    continue;

                int len = OpcodeParameterArray[i].Count;

                if (names.Equals(name, StringComparison.OrdinalIgnoreCase) && len == numParameters)
                    return true;
            }
            return false;
        }

        protected override void FinalizeCompilation(string name, int numParameters, ref List<byte> compiledBytes)
        {
            for (int i = 0; i < OpcodeNames.Length; i++)
            {
                if (OpcodeNames[i] != null && OpcodeNames[i].Equals(name, StringComparison.OrdinalIgnoreCase) && OpcodeParameterArray[i].Count == numParameters)
                {
                    compiledBytes[1] |= (byte)(i << 3);
                    return;
                }
            }
            throw new Exception("Could not find the correct name in finalization"); // should NEVER happen
        }
    }
}
