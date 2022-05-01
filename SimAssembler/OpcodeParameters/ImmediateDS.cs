using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler.OpcodeParameters
{
    internal class ImmediateDS : Immediate
    {
        public ImmediateDS(int size) : base(size) { }

        public override OpcodeReturnInfo Read(OpcodeReadInfo info)
        {
            OpcodeReturnInfo ret = base.Read(info);
            ret.Result = "ds:" + ret.Result;
            return ret;
        }

        public override bool Compile(string parameter, ref List<byte> compiledBytes, ref List<byte> extraFrontBytes, ref List<LinkerRequestEntry> linkerRequests)
        {
            if(parameter.StartsWith("DS:"))
                return base.Compile(parameter.Substring(3).Trim(), ref compiledBytes, ref extraFrontBytes, ref linkerRequests);

            return false;
        }
    }
}
