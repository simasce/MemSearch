using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemSearch
{
	public class SearchTypeConverter
    {
		private static ASCIIEncoding AsciiConverter = new ASCIIEncoding();

		//String representation of 'SearchType' enumerator
		public static string[] SearchTypeStrings =
		{
			"Int8",
			"Int16",
			"Int32",
			"Int64",
			"Float",
			"Double",
			"ASCII String"
		};

		public static byte[] ParseInputToByteArray(string valueInput, SearchType currentType)
		{
			byte[] noret = new byte[0];
			if (valueInput.Length < 1)
				return noret;

			int intResult;
			switch (currentType)
			{
				case SearchType.Int8:
					if (!int.TryParse(valueInput, out intResult))
						return noret;
					byte byteResult = (byte)(intResult & 0xFF);
					return new byte[] { byteResult };
				case SearchType.Int16:
					if (!int.TryParse(valueInput, out intResult))
						return noret;
					short shortResult = (short)(intResult & 0xFFFF);
					return BitConverter.GetBytes(shortResult);
				case SearchType.Int32:
					if (!int.TryParse(valueInput, out intResult))
						return noret;
					return BitConverter.GetBytes(intResult);
				case SearchType.Int64:
					long longResult;
					if (!long.TryParse(valueInput, out longResult))
						return noret;
					return BitConverter.GetBytes(longResult);
				case SearchType.Float:
					float floatResult;
					if (!float.TryParse(valueInput, out floatResult))
						return noret;
					return BitConverter.GetBytes(floatResult);
				case SearchType.Double:
					double doubleResult;
					if (!double.TryParse(valueInput, out doubleResult))
						return noret;
					return BitConverter.GetBytes(doubleResult);
				case SearchType.String:
					return AsciiConverter.GetBytes(valueInput);
			}

			return noret;
		}
	}
}
