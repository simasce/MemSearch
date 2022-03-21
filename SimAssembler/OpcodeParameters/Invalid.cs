using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler.OpcodeParameters
{
    internal class Invalid : OpcodeParameter
    {
        public override OpcodeReturnInfo Read(OpcodeReadInfo info)
        {
            throw new NotImplementedException();
        }

        public override bool Compile(string parameter, ref List<byte> compiledBytes, ref List<byte> extraFrontBytes, ref List<LinkerRequestEntry> linkerRequests)
        {
            throw new NotImplementedException();
        }
    }
}
