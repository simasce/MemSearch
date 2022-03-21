using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimAssembler
{
    internal static class ConversionHelper
    {
        public static bool TryConvertNumericBySize(string str, int size, out byte[] bytes, bool signed=false)
        {
            switch(size)
            {
                case 1:
                    return TryConvertNumericByType(str, typeof(byte), out bytes);
                case 2:
                    return TryConvertNumericByType(str, signed ? typeof(short) : typeof(ushort), out bytes);
                case 4:
                    return TryConvertNumericByType(str, signed ? typeof(int) : typeof(uint), out bytes);
            }

            bytes = null;
            return false;
        }

        public static bool TryConvertNumericByType(string str, Type type, out byte[] bytes)
        {
            string bVal = str;
            bool hex = false;
            if (bVal.StartsWith("0X"))
            {
                hex = true;
                bVal = bVal.Substring(2);
            }

            if(type == typeof(byte))
            {
                byte outByteVal = 0;
                if (hex ? byte.TryParse(bVal, System.Globalization.NumberStyles.HexNumber, null, out outByteVal) : byte.TryParse(bVal, out outByteVal))
                {
                    bytes = BitConverter.GetBytes(outByteVal);
                    return true;
                }
            }
            else if(type == typeof(short))
            {
                short outByteVal = 0;
                if (hex ? short.TryParse(bVal, System.Globalization.NumberStyles.HexNumber, null, out outByteVal) : short.TryParse(bVal, out outByteVal))
                {
                    bytes = BitConverter.GetBytes(outByteVal);
                    return true;
                }
            }
            else if (type == typeof(ushort))
            {
                ushort outByteVal = 0;
                if (hex ? ushort.TryParse(bVal, System.Globalization.NumberStyles.HexNumber, null, out outByteVal) : ushort.TryParse(bVal, out outByteVal))
                {
                    bytes = BitConverter.GetBytes(outByteVal);
                    return true;
                }
            }
            else if (type == typeof(int))
            {
                int outByteVal = 0;
                if (hex ? int.TryParse(bVal, System.Globalization.NumberStyles.HexNumber, null, out outByteVal) : int.TryParse(bVal, out outByteVal))
                {
                    bytes = BitConverter.GetBytes(outByteVal);
                    return true;
                }
            }
            else if (type == typeof(uint))
            {
                uint outByteVal = 0;
                if (hex ? uint.TryParse(bVal, System.Globalization.NumberStyles.HexNumber, null, out outByteVal) : uint.TryParse(bVal, out outByteVal))
                {
                    bytes = BitConverter.GetBytes(outByteVal);
                    return true;
                }
            }
            else if (type == typeof(float))
            {
                float outByteVal = 0;
                if (hex ? float.TryParse(bVal, System.Globalization.NumberStyles.HexNumber, null, out outByteVal) : float.TryParse(bVal, out outByteVal))
                {
                    bytes = BitConverter.GetBytes(outByteVal);
                    return true;
                }
            }

            bytes = null;
            return false;
        }
    }
}
