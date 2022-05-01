using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemSearch
{
    public class DisassemblerEntry
    {
        public string Address { get; set; }
        public string ByteString { get; set; }
        public string Code { get; set; }
        public int Size { get; set; }
    }
}
