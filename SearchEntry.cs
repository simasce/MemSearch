using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemSearch
{
	public enum SearchType
	{
		Int8,
		Int16,
		Int32,
		Int64,
		Float,
		Double,
		String
	}

	public class SearchEntry
	{
		public UInt64 OriginalAddress { get; set; }
		public string Address { get; set; }
		public string Value { get; set; }
		public SearchType ValueType { get; set; }
		public string ValueTypeString { get; set; }
		public bool Frozen { get; set; }
	}
}
