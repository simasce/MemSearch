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
    }
}
