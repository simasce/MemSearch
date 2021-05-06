using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MemSearch
{
	public enum MemorySearchMode
	{
		Byte,
		Int16,
		Int32,
		Int64,
		Float,
		Double,
		String,
		ByteArray
	}

	public class MemorySearchResult
	{
		public List<UInt64> Addresses = new List<UInt64>();
	}

	public class MemorySearcher
	{
		private MemoryProcess TargetProcess;
		private bool ProcessSelected = false;

		private byte[] SearchArray;

		private volatile bool Searching = false;
		private volatile float ReadPercentage = 0.0f;

		private UInt64 BytesRead = 0;
		private UInt64 TargetSize = 0;

		private MemorySearchResult LastResult = new MemorySearchResult();

		private Semaphore ThreadSemaphore = new Semaphore(1, 1);
		private Thread ThreadSearch;

		private ASCIIEncoding ASCIIConverter = new ASCIIEncoding();
		public MemorySearcher()
		{

		}

		public void SelectProcess(MemoryProcess proc)
		{
			if (!proc.IsOpen())
				throw new Exception("Memory process is not open!");

			TargetProcess = proc;
			ProcessSelected = true;
		}

		public MemorySearchResult GetLastResult()
		{
			ThreadSemaphore.WaitOne();
			MemorySearchResult ret = LastResult;
			ThreadSemaphore.Release();
			return ret;
		}

		public float GetSearchPercentage()
		{
			return ReadPercentage;
		}

		public bool IsSearching()
		{
			return Searching;
		}

		private void SearchRegion(UInt64 start, UInt64 end, ref MemorySearchResult result)
		{
			UInt64 baseAddr = start;
			for (; baseAddr < end; baseAddr++)
			{
				try
                {
					byte[] readBytes = TargetProcess.ReadBuffer(baseAddr, SearchArray.Length);
					if (readBytes.SequenceEqual(SearchArray))
					{
						result.Addresses.Add(baseAddr);
					}
				}
				catch(Exception e)
                {
                }
				BytesRead++;
				ReadPercentage = (float)((double)BytesRead / (double)TargetSize);
			}           
		}

		private void SearchThread()
		{
			Searching = true;
			ReadPercentage = 0.0f;
			BytesRead = 0;
			TargetSize = 0;

			MemorySearchResult result = new MemorySearchResult();
			List<MemorySegmentInfo> segments = TargetProcess.EnumerateSegments((UInt64)TargetProcess.GetProcess().MainModule.BaseAddress, 
				TargetProcess.GetProcess().MainModule.ModuleMemorySize);

			foreach (MemorySegmentInfo ms in segments)
				TargetSize += (UInt64)ms.SegmentSize;

			foreach (MemorySegmentInfo ms in segments)
				SearchRegion(ms.SegmentStart, ms.SegmentStart + ms.SegmentSize, ref result);

			LastResult = result;
			Searching = false;
			ThreadSemaphore.Release();
		}

		private void SearchAgainThread()
		{
			Searching = true;
			ReadPercentage = 0.0f;
			BytesRead = 0;
			TargetSize = 0;

			MemorySearchResult result = new MemorySearchResult();
			for (int i = 0; i < LastResult.Addresses.Count; i++)
			{
				UInt64 addy = LastResult.Addresses[i];

				byte[] readBytes = TargetProcess.ReadBuffer(addy, SearchArray.Length);
				if (readBytes.SequenceEqual(SearchArray))
				{
					result.Addresses.Add(addy);
				}

				ReadPercentage = (float)(i + 1) / (float)LastResult.Addresses.Count;
			}

			LastResult = result;
			Searching = false;
			ThreadSemaphore.Release();
		}

		private void CreateSearch(byte[] searchBytes)
		{
			if (!ProcessSelected)
				throw new Exception("Process not selected!");

			if (!TargetProcess.IsOpen())
				throw new Exception("Process is not open!");

			ThreadSemaphore.WaitOne();
			LastResult.Addresses.Clear();
			SearchArray = searchBytes;
			ThreadSearch = new Thread(SearchThread);
			ThreadSearch.Start();
		}

		public void SearchAgain(byte[] searchBytes)
		{        
			if (!ProcessSelected)
				throw new Exception("Process not selected!");

			if (!TargetProcess.IsOpen())
				throw new Exception("Process is not open!");

			ThreadSemaphore.WaitOne();
			SearchArray = searchBytes;

			if (LastResult.Addresses.Count < 1 || SearchArray.Length < 1)
			{
				ThreadSemaphore.Release();
				return; //no search needed
			}              
		 
			ThreadSearch = new Thread(SearchAgainThread);
			ThreadSearch.Start();
		}

		public void SearchByte(byte sByte)
		{
			byte[] sBuf = { sByte };
			CreateSearch(sBuf);
		}

		public void SearchInt16(Int16 i16)
		{
			byte[] sBuf = BitConverter.GetBytes(i16);
			CreateSearch(sBuf);
		}

		public void SearchInt32(Int32 i32)
		{
			byte[] sBuf = BitConverter.GetBytes(i32);
			CreateSearch(sBuf);
		}

		public void SearchInt64(Int64 i64)
		{
			byte[] sBuf = BitConverter.GetBytes(i64);
			CreateSearch(sBuf);
		}

		public void SearchFloat(float fl)
		{
			byte[] sBuf = BitConverter.GetBytes(fl);
			CreateSearch(sBuf);
		}

		public void SearchDouble(double dl)
		{
			byte[] sBuf = BitConverter.GetBytes(dl);
			CreateSearch(sBuf);
		}

		public void SearchString(string str)
		{
			byte[] sBuf = ASCIIConverter.GetBytes(str);
			CreateSearch(sBuf);
		}

		public void SearchByteArray(byte[] array)
		{
			CreateSearch(array);
		}
	}
}
