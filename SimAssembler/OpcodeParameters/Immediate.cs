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
            int curSize = Size;

            if (info.OverrideByte == 0x66) //lower size prefix
                curSize = Math.Max(1, curSize / 2);

            MemoryStream ms = info.Stream;
            if (ms.Position + curSize > ms.Length)
                throw new IndexOutOfRangeException();

            byte[] buffer = new byte[curSize];
            ms.Read(buffer, 0, curSize);

            return new OpcodeReturnInfo()
            {
                Result = "0x" + string.Join("", buffer.Reverse().Select(f => f.ToString("X2"))),
                Bytes = buffer,
                Offset = info.Offset
            };
        }
    }
}
