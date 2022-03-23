using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MemSearch
{
	public enum ProcessAccessFlags : uint
	{
		All = 0x001F0FFF,
		Terminate = 0x00000001,
		CreateThread = 0x00000002,
		VirtualMemoryOperation = 0x00000008,
		VirtualMemoryRead = 0x00000010,
		VirtualMemoryWrite = 0x00000020,
		DuplicateHandle = 0x00000040,
		CreateProcess = 0x000000080,
		SetQuota = 0x00000100,
		SetInformation = 0x00000200,
		QueryInformation = 0x00000400,
		QueryLimitedInformation = 0x00001000,
		Synchronize = 0x00100000
	}

	public enum BinaryType : uint
	{
		SCS_32BIT_BINARY = 0,   // A 32-bit Windows-based application
		SCS_64BIT_BINARY = 6,   // A 64-bit Windows-based application.
		SCS_DOS_BINARY = 1,     // An MS-DOS � based application
		SCS_OS216_BINARY = 5,   // A 16-bit OS/2-based application
		SCS_PIF_BINARY = 3,     // A PIF file that executes an MS-DOS � based application
		SCS_POSIX_BINARY = 4,   // A POSIX � based application
		SCS_WOW_BINARY = 2      // A 16-bit Windows-based application
	}

	public enum MemoryProtection : uint
	{
		PAGE_EXECUTE = 0x10,
		PAGE_EXECUTE_READ = 0x20,
		PAGE_EXECUTE_READWRITE = 0x40,
		PAGE_EXECUTE_WRITECOPY = 0x80,
		PAGE_NOACCESS = 0x01,
		PAGE_READONLY = 0x02,
		PAGE_READWRITE = 0x04,
		PAGE_WRITECOPY = 0x08
	}

	public enum MemoryState : uint
	{
		MEM_COMMIT = 0x00001000,
		MEM_RESERVE = 0x00002000,
		MEM_RESET = 0x00080000,
		MEM_RESET_UNDO = 0x1000000,
		MEM_LARGE_PAGES = 0x20000000,
		MEM_PHYSICAL = 0x00400000,
		MEM_TOP_DOWN = 0x00100000
	}

	public struct MEMORY_BASIC_INFORMATION
	{
		public int BaseAddress;
		public int AllocationBase;
		public int AllocationProtect;
		public int RegionSize;
		public int State;
		public int Protect;
		public int lType;
	}

	public struct MEMORY_BASIC_INFORMATION64
	{
		public ulong BaseAddress;
		public ulong AllocationBase;
		public int AllocationProtect;
		public int __alignment1;
		public ulong RegionSize;
		public int State;
		public int Protect;
		public int Type;
		public int __alignment2;
	}

	class MemoryAPI
	{
		[DllImport("kernel32.dll")]
		public static extern uint GetLastError();

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer,int dwSize, out IntPtr lpNumberOfBytesRead);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

		[DllImport("kernel32.dll")]
		public static extern bool GetBinaryType(string lpApplicationName, out BinaryType lpBinaryType);

		[DllImport("kernel32.dll")]
		public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress,  UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

		[DllImport("kernel32.dll", SetLastError = true, EntryPoint = "VirtualQueryEx")]
		public static extern int VirtualQueryEx32(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);
		
		[DllImport("kernel32.dll", SetLastError = true, EntryPoint = "VirtualQueryEx")]
		public static extern int VirtualQueryEx64(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION64 lpBuffer, uint dwLength);

		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);
		public static IntPtr OpenProcess(Process proc, ProcessAccessFlags flags)
		{
			return OpenProcess((uint)flags, false, proc.Id);
		}
	}
}
