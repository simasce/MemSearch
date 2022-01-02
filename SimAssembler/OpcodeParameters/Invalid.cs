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
    }
}
