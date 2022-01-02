using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler.OpcodeParameters
{
    internal class None : OpcodeParameter
    {
        public override OpcodeReturnInfo Read(OpcodeReadInfo info)
        {
            return new OpcodeReturnInfo()
            {
                Bytes = new byte[0],
                Offset = info.Offset,
                Result = ""
            };
        }
    }
}
