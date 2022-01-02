using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler.OpcodeParameters
{
    internal class Static : OpcodeParameter
    {
        public string Name { get; private set; }
        public Static(string Name)
        {
            this.Name = Name;
        }

        public override OpcodeReturnInfo Read(OpcodeReadInfo info)
        {
            return new OpcodeReturnInfo()
            {
                Offset = info.Offset,
                Bytes = new byte[0],
                Result = Name
            };
        }
    }
}
