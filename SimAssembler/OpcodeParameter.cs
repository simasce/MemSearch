using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler
{
    internal abstract class OpcodeParameter
    {
        public abstract OpcodeReturnInfo Read(OpcodeReadInfo info);
    }
}
