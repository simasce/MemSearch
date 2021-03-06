using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler.OpcodeParameters
{
    internal class FarPointer : OpcodeParameter
    {
        public int HeaderSize { get; private set; }
        public int BodySize { get; private set; }

        public FarPointer(int headerSize, int bodySize)
        {
            HeaderSize = headerSize;
            BodySize = bodySize;
        }

        public override OpcodeReturnInfo Read(OpcodeReadInfo info)
        {
            int size = HeaderSize + BodySize;
            MemoryStream ms = info.Stream;
            if (ms.Position + size > ms.Length)
                throw new IndexOutOfRangeException();

            byte[] buffer = new byte[size];
            ms.Read(buffer, 0, size);

            if (BitConverter.IsLittleEndian) //inverted reversal because we read consequent hex bytes for immediats
                Array.Reverse(buffer);

            byte[] header = buffer.Take(HeaderSize).ToArray();
            byte[] body = buffer.Skip(HeaderSize).ToArray();

            string str_header = "0x" + string.Join("", header.Select(x => x.ToString("X2")));
            string str_body = "0x" + string.Join("", body.Select(x => x.ToString("X2")));

            return new OpcodeReturnInfo()
            {
                Result = str_header + ":" + str_body,
                Bytes = buffer.ToList(),
                Offset = info.Offset
            };
        }

        public override bool Compile(string parameter, ref List<byte> compiledBytes, ref List<byte> extraFrontBytes, ref List<LinkerRequestEntry> linkerRequests)
        {
            string[] subParams = parameter.Split(':');
            if (subParams.Length != 2)
                return false;

            byte[] compiled_header;
            byte[] compiled_body;
            if(!ConversionHelper.TryConvertNumericBySize(subParams[0], HeaderSize, out compiled_header) ||
               !ConversionHelper.TryConvertNumericBySize(subParams[1], BodySize, out compiled_body))
            {
                return false;
            }

            compiledBytes.AddRange(compiled_header);
            compiledBytes.AddRange(compiled_body);
            return true;
        }
    }
}
