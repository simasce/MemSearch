using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler.OpcodeParameters
{
    internal class Immediate : OpcodeParameter
    {
        public int Size { get; private set; }
        public Immediate(int size_bytes)
        {
            this.Size = size_bytes;
        }

        public override OpcodeReturnInfo Read(OpcodeReadInfo info)
        {
            MemoryStream ms = info.Stream;
            if (ms.Position + Size > ms.Length)
                throw new IndexOutOfRangeException();

            byte[] buffer = new byte[Size];
            ms.Read(buffer, 0, Size);

            Array.Reverse(buffer);

            return new OpcodeReturnInfo()
            {
                Result = "0x" + string.Join("", buffer.Select(f => f.ToString("X2"))),
                Bytes = buffer,
                Offset = info.Offset
            };
        }
    }
}
