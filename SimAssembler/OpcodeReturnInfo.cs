using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler
{
    public class OpcodeReturnInfo
    {
        public string Result { get; set; }
        public UInt64 Offset { get; set; } // Offset is set by handler
        public byte[] Bytes { get; set; }  // Bytes is set by OpcodeParameter but then appended to front by handler

        public override string ToString()
        {
            return Offset.ToString("X") + " [" + string.Join(' ', Bytes.Select(f => f.ToString("X2"))) + "] " + Result;
        }
    }
}
