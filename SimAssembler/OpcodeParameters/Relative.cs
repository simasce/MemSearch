using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler.OpcodeParameters
{
    internal class Relative : OpcodeParameter
    {
        public int Size { get; private set; }
        public Relative(int size_bytes)
        {
            this.Size = size_bytes;
        }

        public override OpcodeReturnInfo Read(OpcodeReadInfo info)
        {
            MemoryStream ms = info.Stream;
            if (ms.Position + Size > ms.Length)
                throw new IndexOutOfRangeException();

            byte[] buffer = new byte[Size];
            byte[] buffer_copy = new byte[Size]; // for output

            ms.Read(buffer, 0, Size);

            Array.Copy(buffer, buffer_copy, Size);

            if (!BitConverter.IsLittleEndian) //only need reversing where we actually use the numeric value for arithmetics
                Array.Reverse(buffer);

            ulong offset = info.Offset + (ulong)buffer.Length;

            ulong result = 0;
            switch(Size)
            {
                case 1:
                    result = offset + (UInt64)(byte)buffer[0];
                    break;
                case 2:
                    result = offset + (UInt64)BitConverter.ToUInt16(buffer);
                    break;
                case 4:
                    result = offset + (UInt64)BitConverter.ToUInt32(buffer);
                    break;
                case 8:
                    result = offset + (UInt64)BitConverter.ToUInt64(buffer);
                    break;
            }

            return new OpcodeReturnInfo()
            {
                Bytes = buffer_copy.ToList(),
                Offset = info.Offset,
                Result = result.ToString("X8")
            };
        }

        public override bool Compile(string parameter, ref List<byte> compiledBytes, ref List<byte> extraFrontBytes, ref List<LinkerRequestEntry> linkerRequests)
        {
            bool hex = false;
            string bVal = parameter;
            if (parameter.StartsWith("0X"))
            {
                bVal = parameter.Substring(2);
                hex = true;
            }

            int temp = 0;
            if((hex ? !int.TryParse(bVal, System.Globalization.NumberStyles.HexNumber, null, out temp) : !int.TryParse(bVal, out temp)))
            {   
                linkerRequests.Add(new LinkerRequestEntry()
                {
                    Offset = compiledBytes.Count + extraFrontBytes.Count,
                    PointerName = bVal,
                    Size = this.Size,
                    Relative = true
                });
                compiledBytes.AddRange(Enumerable.Repeat<byte>(0x00, this.Size));
                return true;
            }

            byte[] compiled;
            if (ConversionHelper.TryConvertNumericBySize(parameter, Size, out compiled))
            {
                compiledBytes.AddRange(compiled);
                return true;
            }

            return false;
        }
    }
}
