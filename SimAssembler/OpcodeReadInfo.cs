using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimAssembler.OpcodeParameters;

namespace SimAssembler
{
    internal class OpcodeReadInfo
    {
        public MemoryStream Stream { get; set; }
        public UInt64 Offset { get; set; }
        public byte OverrideByte { get; set; } //Extra byte for parameters such as 0x66 which lowers param size
        //Add x86/64 bit switch later
    }
}
