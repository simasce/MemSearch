using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler
{
    internal class LinkerRequestEntry
    {
        public OpcodeReturnInfo CompiledOpcode;
        public int Offset;
        public int Size; 
        public string PointerName;
        public bool Relative;
    }
}
