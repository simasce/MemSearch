using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SimAssembler
{
    internal class Opcode
    {
        public byte Opbyte { get; set; }
        public string Name { get; set; }
        public List<OpcodeParameter> Parameters { get; set; }

        public virtual string GetName(MemoryStream ms)
        {
            return Name;
        }

        public virtual List<OpcodeParameter> GetParameters(MemoryStream ms)
        {
            return Parameters;
        }
    }
}
