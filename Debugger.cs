using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MemSearch
{
    public delegate void DebuggerCallbackEntry(Debugger thisDebugger, ref CONTEXT threadContext);

    public class Debugger : IDisposable
    {
        private static bool EnabledDebugPrivilege = false;
               
        public MemoryProcess Process { get { return _memoryProcess; } }
        private MemoryProcess _memoryProcess;

        private int _threadId = 0;
        private IntPtr _threadHandle = IntPtr.Zero;

        private bool _initialized = false;
        private Dictionary<UInt64, DebuggerCallbackEntry> _entries = new Dictionary<UInt64, DebuggerCallbackEntry>();

        private Thread _debuggerThread = null;
        private volatile bool _debuggerWorking = false;
        
        private Mutex _mutex = new Mutex();

        private CONTEXT _pauseContext = new CONTEXT();
        private volatile bool _debugHalt = false;

        public Debugger(MemoryProcess proc)
        {
            _memoryProcess = proc;
        }

        public void AddBreakpoint(UInt64 address, DebuggerCallbackEntry callback)
        {
            if (_entries.Count >= 4)
                return;

            _mutex.WaitOne();
            _entries.Add(address, callback);
            _mutex.ReleaseMutex();

            UpdateBreakpointRegisters();
        }

        public void RemoveBreakpoint(UInt64 address)
        {
            _mutex.WaitOne();
            _entries.Remove(address);
            _mutex.ReleaseMutex();

            UpdateBreakpointRegisters();
        }

        public bool Initialize()
        {
            if (_initialized)
                return true;

            if (!EnabledDebugPrivilege)
            {
                if (!SetDebugPrivilege(true))
                    return false;
            }

            _debuggerThread = new Thread(ThreadLoop);
            _debuggerThread.Start();    

            return true;
        }

        public void Stop()
        {
            if (_debuggerThread != null)
            {
                _debuggerWorking = false;
                _debuggerThread.Join();
            }
            _debuggerThread = null;
            _entries.Clear();
        }

        private void UpdateBreakpointRegisters()
        {
            _mutex.WaitOne();

            MemoryAPI.SuspendThread(_threadHandle);

            CONTEXT ctx = new CONTEXT();          
            if(_debugHalt)
            {
                ctx = _pauseContext;
            }
            else
            {
                ctx.ContextFlags = 65557;
                if (!MemoryAPI.GetThreadContext(_threadHandle, ref ctx))
                {
                    _mutex.ReleaseMutex();
                    MemoryAPI.ResumeThread(_threadHandle);
                    return;
                }
            }

            ctx.Dr0 = 0;
            ctx.Dr1 = 0;
            ctx.Dr2 = 0;
            ctx.Dr3 = 0;
            ctx.Dr7 = 0;         

            for(int i = 0; i < _entries.Count; i++)
            {
                var e = _entries.ElementAt(i);
                switch (i)
                {
                    case 0:
                        ctx.Dr0 = (uint)e.Key;
                        ctx.Dr7 |= 1;
                        break;
                    case 1:
                        ctx.Dr1 = (uint)e.Key;
                        ctx.Dr7 |= 4;
                        break;
                    case 2:
                        ctx.Dr2 = (uint)e.Key;
                        ctx.Dr7 |= 16;
                        break;
                    case 3:
                        ctx.Dr3 = (uint)e.Key;
                        ctx.Dr7 |= 64;
                        break;
                }               
            }

            if (!_debugHalt)
            {
                MemoryAPI.SetThreadContext(_threadHandle, ref ctx);
                MemoryAPI.ResumeThread(_threadHandle);
            }
            _mutex.ReleaseMutex();
        }

        private void ClearDebugRegisters()
        {
            _mutex.WaitOne();
            MemoryAPI.SuspendThread(_threadHandle);

            CONTEXT ctx = new CONTEXT();
            ctx.ContextFlags = 65557;
            if (!MemoryAPI.GetThreadContext(_threadHandle, ref ctx))
            {
                _mutex.ReleaseMutex();
                MemoryAPI.ResumeThread(_threadHandle);
                return;
            }

            ctx.Dr0 = 0;
            ctx.Dr1 = 0;
            ctx.Dr2 = 0;
            ctx.Dr3 = 0;
            ctx.Dr7 = 0;

            _mutex.ReleaseMutex();

            MemoryAPI.SetThreadContext(_threadHandle, ref ctx);
            MemoryAPI.ResumeThread(_threadHandle);
        }

        private void ThreadLoop()
        {
            uint processId = (uint)_memoryProcess.GetProcess().Id;
            if (!OpenMainThread() || !MemoryAPI.DebugActiveProcess(processId))
            {
                _debuggerWorking = false;
                return;
            }

            int repeats = 0;

            _debuggerWorking = true;
            DEBUG_EVENT dEvent = new DEBUG_EVENT();
            MemoryAPI.DebugSetProcessKillOnExit(false);
            UpdateBreakpointRegisters();
            while (_debuggerWorking && _memoryProcess.IsOpen())
            {
                if (!MemoryAPI.WaitForDebugEvent(ref dEvent, 500))
                {
                    if(++repeats >= 5)
                    {
                        repeats = 0;
                        UpdateBreakpointRegisters();
                    }
                    continue;
                }

                if(dEvent.dwDebugEventCode == DebugEventType.EXCEPTION_DEBUG_EVENT)
                {                    
                    DebuggerCallbackEntry deb;
                    _mutex.WaitOne();
                    bool handled = _entries.TryGetValue(dEvent.address, out deb);
                    _mutex.ReleaseMutex();

                    if (handled)
                    {
                        _pauseContext.ContextFlags = 65558;
                        _pauseContext.Eip = dEvent.address;
                        MemoryAPI.GetThreadContext(_threadHandle, ref _pauseContext);
                        deb.Invoke(this, ref _pauseContext);
                        while(_debugHalt) { Thread.Sleep(10); }
                        MemoryAPI.SetThreadContext(_threadHandle, ref _pauseContext);
                        ClearDebugRegisters();
                        MemoryAPI.ContinueDebugEvent(dEvent.dwProcessId, dEvent.dwThreadId, ContinueStatus.DBG_CONTINUE);
                        Thread.Sleep(10);
                        UpdateBreakpointRegisters();
                    }
                    else
                    {
                        MemoryAPI.ContinueDebugEvent(dEvent.dwProcessId, dEvent.dwThreadId, ContinueStatus.DBG_EXCEPTION_NOT_HANDLED);
                    }                  
                }
                else
                {
                    MemoryAPI.ContinueDebugEvent(dEvent.dwProcessId, dEvent.dwThreadId, ContinueStatus.DBG_CONTINUE);
                }   
            }

            if (_memoryProcess.IsOpen())
                ClearDebugRegisters();

            CloseMainThread();
            MemoryAPI.DebugActiveProcessStop(processId);
            _debuggerWorking = false;
        }

        public void HaltDebugEvent()
        {
            _debugHalt = true;
        }

        public void ContinueDebugEvent()
        {
            _debugHalt = false;
        }

        public void SetContext(CONTEXT ctx)
        {
            if (_debugHalt)
            {
                _pauseContext = ctx;
            }
            else
            {
                MemoryAPI.SuspendThread(_threadHandle);
                MemoryAPI.SetThreadContext(_threadHandle, ref ctx);
                MemoryAPI.ResumeThread(_threadHandle);
            }
        }

        private bool OpenMainThread()
        {
            var proc = _memoryProcess.GetProcess();
            IntPtr windowHandle = proc.MainWindowHandle;

            _threadId = MemoryAPI.GetWindowThreadProcessId(windowHandle, IntPtr.Zero);
            foreach (System.Diagnostics.ProcessThread e in proc.Threads)
            {
                if(e.Id == _threadId)
                {
                    _threadHandle = MemoryAPI.OpenThread((ThreadAccess)1048666, false, (uint)e.Id);
                    return _threadHandle != IntPtr.Zero;
                }
            }

            return false;
        }

        private void CloseMainThread()
        {
            if(_threadHandle != IntPtr.Zero)
            {
                MemoryAPI.CloseHandle(_threadHandle);
                _threadHandle = IntPtr.Zero;
            }
        }

        void IDisposable.Dispose()
        {
            Stop();      
        }

        public static bool SetDebugPrivilege(bool enabled)
        {
            const int SE_PRIVILEGE_DISABLED = 0x00000000;
            const int SE_PRIVILEGE_ENABLED = 0x00000002;
            const UInt32 TOKEN_ADJUST_PRIVILEGES = 0x0020;
            const UInt32 TOKEN_QUERY = 0x0008;

            try
            {
                TOKEN_PRIVILEGES tp;
                IntPtr hProcess = MemoryAPI.GetCurrentProcess();
                IntPtr hToken = IntPtr.Zero;
                if (!MemoryAPI.OpenProcessToken(hProcess, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out hToken))
                    return false;

                tp.PrivilegeCount = 1;
                tp.Luid.LowPart = 0;
                tp.Luid.HighPart = 0;
                tp.Attributes = enabled ? SE_PRIVILEGE_ENABLED : SE_PRIVILEGE_DISABLED;
                if (!MemoryAPI.LookupPrivilegeValue(null, "SeDebugPrivilege", ref tp.Luid))
                {
                    MemoryAPI.CloseHandle(hProcess);
                    return false;
                }
                    
                if(!MemoryAPI.AdjustTokenPrivileges(hToken, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero))
                {
                    MemoryAPI.CloseHandle(hProcess);
                    return false;
                }
               
                MemoryAPI.CloseHandle(hProcess);
            }
            catch(Exception ex)
            {
                return false;
            }
            EnabledDebugPrivilege = enabled;
            return true;
        }
    }
}
