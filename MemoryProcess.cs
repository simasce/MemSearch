using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MemSearch
{
	public class MemorySegmentInfo
	{
		public UInt64 SegmentStart;
		public UInt64 SegmentSize;
	}

	public class MemoryProcess : IDisposable
	{
		private Process CurrentProcess;

		private IntPtr ProcessHandle = IntPtr.Zero;
		private bool ProcessOpen = false;
		private bool Is64BitProcess = false;

		private ASCIIEncoding ASCIIConverter = new ASCIIEncoding();
		public MemoryProcess(Process process)
		{
			CurrentProcess = process;
			Open();
		}

		~MemoryProcess()
		{
			Close(false);
		}

		private void Open()
		{
			string processPath = CurrentProcess.MainModule.FileName;
			BinaryType processBinaryType;

			if (!MemoryAPI.GetBinaryType(processPath, out processBinaryType))
				throw new Exception("Failed to get process binary type!");

			switch (processBinaryType)
			{
				case BinaryType.SCS_32BIT_BINARY:
					Is64BitProcess = false;
					break;
				case BinaryType.SCS_64BIT_BINARY:
					Is64BitProcess = true;
					break;
				default:
					throw new Exception("Unsupported process type!");
			}

			ProcessHandle = MemoryAPI.OpenProcess(CurrentProcess, ProcessAccessFlags.All);

			if (ProcessHandle == IntPtr.Zero)
				throw new Exception("Failed to open process!");

			ProcessOpen = true;
		}

		public void Close(bool GC_Call)
		{
			if (!ProcessOpen)
				return;

			MemoryAPI.CloseHandle(ProcessHandle);
			ProcessHandle = IntPtr.Zero;
			ProcessOpen = false;
		}

		public Process GetProcess()
		{
			return CurrentProcess;
		}

		public bool Is64Bit()
		{
			return Is64BitProcess;
		}

		public bool IsOpen()
		{
			return ProcessOpen && !CurrentProcess.HasExited;
		}

		public byte[] ReadBuffer(UInt64 address, int size)
		{
			if (!IsOpen())
				throw new Exception("Process is not open!");

			IntPtr targetAddress = (IntPtr)address;
			IntPtr numRead;
			byte[] returnBytes = new byte[size];
			if (!MemoryAPI.ReadProcessMemory(ProcessHandle, targetAddress, returnBytes, size, out numRead))
				throw new Exception("Failed reading memory!");
			if ((int)numRead != size)
				throw new Exception("Could not read enough bytes!");
			return returnBytes;
		}

		public void WriteBuffer(UInt64 address, byte[] bytes)
		{
			if (!IsOpen())
				throw new Exception("Process is not open!");

			IntPtr targetAddress = (IntPtr)address;
			IntPtr numWrite;

			uint oldProtect, oldProtect2;
			MemoryAPI.VirtualProtectEx(ProcessHandle, targetAddress, (UIntPtr)bytes.Length, (uint)MemoryProtection.PAGE_EXECUTE_READWRITE, out oldProtect);

			if (!MemoryAPI.WriteProcessMemory(ProcessHandle, targetAddress, bytes, bytes.Length, out numWrite))
			{
				MemoryAPI.VirtualProtectEx(ProcessHandle, targetAddress, (UIntPtr)bytes.Length, oldProtect, out oldProtect2);
				throw new Exception("Failed writing memory!");
			}

			if ((int)numWrite != bytes.Length)
			{
				MemoryAPI.VirtualProtectEx(ProcessHandle, targetAddress, (UIntPtr)bytes.Length, oldProtect, out oldProtect2);
				throw new Exception("Could not write enough bytes!");
			}

			MemoryAPI.VirtualProtectEx(ProcessHandle, targetAddress, (UIntPtr)bytes.Length, oldProtect, out oldProtect2);
		}

		public void Dispose()
		{
			Close(true);
			GC.SuppressFinalize(this);
		}

		private List<MemorySegmentInfo> EnumerateSegments32(UInt64 moduleBaseAddress, int moduleLength)
        {
			List<MemorySegmentInfo> segments = new List<MemorySegmentInfo>();

			MEMORY_BASIC_INFORMATION memInfo = new MEMORY_BASIC_INFORMATION();

			UInt64 addressIter = moduleBaseAddress;
			UInt64 addressEnd = moduleBaseAddress + (UInt64)moduleLength;

			while (addressIter < addressEnd)
			{
				if (MemoryAPI.VirtualQueryEx32(ProcessHandle, (IntPtr)addressIter, out memInfo, 28) == 0)//28 = sizeof(MEMORY_BASIC_INFORMATION)
					throw new Exception("VirtualQueryEx failed! " + MemoryAPI.GetLastError().ToString());			

				if (memInfo.State == (int)MemoryState.MEM_COMMIT)
				{
					segments.Add(new MemorySegmentInfo() { SegmentStart = (UInt64)memInfo.BaseAddress, SegmentSize = (UInt64)memInfo.RegionSize });
				}
				addressIter += (UInt64)memInfo.RegionSize;
			}

			return segments;
		}

		private List<MemorySegmentInfo> EnumerateSegments64(UInt64 moduleBaseAddress, int moduleLength)
		{
			List<MemorySegmentInfo> segments = new List<MemorySegmentInfo>();

			MEMORY_BASIC_INFORMATION64 memInfo = new MEMORY_BASIC_INFORMATION64();

			UInt64 addressIter = moduleBaseAddress;
			UInt64 addressEnd = moduleBaseAddress + (UInt64)moduleLength;

			while (addressIter < addressEnd)
			{
				if (MemoryAPI.VirtualQueryEx64(ProcessHandle, (IntPtr)addressIter, out memInfo, 48) == 0)//48 = sizeof(MEMORY_BASIC_INFORMATION64)
					throw new Exception("VirtualQueryEx failed! " + MemoryAPI.GetLastError().ToString());

				if (memInfo.State == (int)MemoryState.MEM_COMMIT)
				{
					segments.Add(new MemorySegmentInfo() { SegmentStart = (UInt64)memInfo.BaseAddress, SegmentSize = memInfo.RegionSize });
				}
				addressIter += (UInt64)memInfo.RegionSize;
			}

			return segments;
		}

		public List<MemorySegmentInfo> EnumerateSegments(UInt64 moduleBaseAddress, int moduleLength)
		{
			if (IntPtr.Size == 4) //our own program environment is the one to consider.
				return EnumerateSegments32(moduleBaseAddress, moduleLength);
			return EnumerateSegments64(moduleBaseAddress, moduleLength);
		}

		public byte ReadByte(UInt64 address)
		{
			byte[] buf = ReadBuffer(address, 1);
			return buf[0];
		}

		public Int16 ReadInt16(UInt64 address)
		{
			byte[] buf = ReadBuffer(address, 2);
			return BitConverter.ToInt16(buf);
		}

		public Int32 ReadInt32(UInt64 address)
		{
			byte[] buf = ReadBuffer(address, 4);
			return BitConverter.ToInt32(buf);
		}

		public Int64 ReadInt64(UInt64 address)
		{
			byte[] buf = ReadBuffer(address, 8);
			return BitConverter.ToInt64(buf);
		}

		public float ReadFloat(UInt64 address)
		{
			byte[] buf = ReadBuffer(address, 4);
			return BitConverter.ToSingle(buf);
		}

		public double ReadDouble(UInt64 address)
		{
			byte[] buf = ReadBuffer(address, 8);
			return BitConverter.ToDouble(buf);
		}

		public string ReadString(UInt64 address, int size) //Reads ASCII String
		{
			byte[] buf = ReadBuffer(address, size);
			return ASCIIConverter.GetString(buf);
		}

		public string ReadStringToEnd(UInt64 address) //Read ASCII String untill reaches null byte
		{
			List<byte> byteBuffer = new List<byte>();
			UInt64 iterator = 0;
			byte readByte = 0;
			while (true)
			{
				readByte = ReadByte(address + iterator);
				if (readByte == 0x00)
					break;
				byteBuffer.Add(readByte);
				iterator++;
			}
			byte[] outBytes = byteBuffer.ToArray();
			return ASCIIConverter.GetString(outBytes);
		}

		public void WriteByte(UInt64 address, byte byt)
		{
			byte[] wBuffer = { byt };
			WriteBuffer(address, wBuffer);
		}

		public void WriteInt16(UInt64 address, Int16 i16)
		{
			byte[] wBuffer = BitConverter.GetBytes(i16);
			WriteBuffer(address, wBuffer);
		}

		public void WriteInt32(UInt64 address, Int32 i32)
		{
			byte[] wBuffer = BitConverter.GetBytes(i32);
			WriteBuffer(address, wBuffer);
		}

		public void WriteInt64(UInt64 address, Int64 i64)
		{
			byte[] wBuffer = BitConverter.GetBytes(i64);
			WriteBuffer(address, wBuffer);
		}

		public void WriteFloat(UInt64 address, float fl)
		{
			byte[] wBuffer = BitConverter.GetBytes(fl);
			WriteBuffer(address, wBuffer);
		}

		public void WriteDouble(UInt64 address, double dl)
		{
			byte[] wBuffer = BitConverter.GetBytes(dl);
			WriteBuffer(address, wBuffer);
		}

		public void WriteString(UInt64 address, string str)
		{
			byte[] wBuffer = ASCIIConverter.GetBytes(str);
			WriteBuffer(address, wBuffer);
		}
	}
}
