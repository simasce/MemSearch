using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using SimAssembler;
using System.Threading;
using System.Windows.Controls;

namespace MemSearch
{
    /// <summary>
    /// Interaction logic for Disassembler.xaml
    /// </summary>
    public partial class Disassembler : Window
    {
        private class RegisterValueEntry
        {
            public string Name { get; set; }
            public string ValueHex { get; set; }
            public ulong Backup { get; set; }
        }

        private class BreakpointEntry
        {
            public string Address { get; set; }
            public string Count { get; set; }
            public string Time { get; set; }
            public string BreakpointHit { get; set; }
            public DateTime LastTime { get; set; }
        }

        private class DisassemblerValueEntry : DisassemblerEntry
        {
            public string BreakpointHit { get; set; }
        }

        private MemoryProcess TargetProcess { get; set; }
        private Debugger TargetDebugger { get; set; }

        private Mutex ContextMutex = new Mutex();
        private CONTEXT CurrentContext = new CONTEXT();
        private volatile bool IsExecutionPaused = false;
        private volatile bool IsWaitingForPause = false;

        private List<RegisterValueEntry> RegisterValues = new List<RegisterValueEntry>();

        public Disassembler(MemoryProcess targetProcess)
        {
            TargetProcess = targetProcess;
            TargetDebugger = targetProcess.GetDebugger();

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += WindowProcessTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();
          
            InitializeComponent();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void WindowProcessTimer_Tick(object sender, EventArgs e)
        {
            if (TargetProcess == null)
                return;

            bool open = TargetProcess.IsOpen();

            //sneaky switch check
            if (!open && UpdateButton.IsEnabled)
            {
                DisassemblerDataGrid.Items.Clear();
                BreakpointDataGrid.Items.Clear();
                RegisterValues.Clear();
                DebuggerDataGrid.Items.Refresh();
            }

            UpdateButton.IsEnabled = open;           
            DebuggerAddBreakpointButton.IsEnabled = open;

            if(!open)
                DebuggerContinueButton.IsEnabled = open;

            ContextMutex.WaitOne();
            if(IsWaitingForPause && IsExecutionPaused)
            {
                ThisThreadDebuggerCallback();
                IsWaitingForPause = false;
            }
            ContextMutex.ReleaseMutex();
        }

        public void SetTargetProcess(MemoryProcess proc)
        {
            TargetProcess = proc;
        }

        private void Update()
        {
            ulong startAddress = 0;
            int size = 0;

            if (!TargetProcess.IsOpen())
                return;

            if (!ulong.TryParse(StartAddressTextBox.Text, NumberStyles.HexNumber, null, out startAddress) ||
                !int.TryParse(SizeTextBox.Text, out size))
                return;

            DisassemblerDataGrid.Items.Clear();

            try
            {
                byte[] readBuffer = TargetProcess.ReadBuffer(startAddress, size);

                bool finished = false;
                var disassembled = SimAssembler.Assembler.Disassemble(readBuffer, out finished, startAddress);
                foreach (var dis in disassembled)
                {
                    var entry = new DisassemblerValueEntry()
                    {
                        Address = dis.Offset.ToString("X"),
                        ByteString = string.Join(' ', dis.Bytes.Select(f => f.ToString("X2"))),
                        Code = dis.Result,
                        Size = dis.Bytes.Count,
                        BreakpointHit = ""
                    };
                    DisassemblerDataGrid.Items.Add(entry);
                }
            }
            catch (Exception)
            { 
            }
        }

        private void DisassemblerDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DisassemblerDataGrid.SelectedItem == null)
                return;

            new Assembler((DisassemblerEntry)DisassemblerDataGrid.SelectedItem, TargetProcess).ShowDialog();

            Update();
        }

        private void InjectCodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (DisassemblerDataGrid.SelectedItem == null)
                return;

            new CodeInjectionWindow(TargetProcess, (DisassemblerEntry)DisassemblerDataGrid.SelectedItem).ShowDialog();

            Update();
        }

        private void DisassemblerDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InjectCodeButton.IsEnabled = (DisassemblerDataGrid.SelectedItem != null);
            DebuggerAddBreakpointButton.IsEnabled = (DisassemblerDataGrid.SelectedItem != null);
        }

        private void UpdateRegisterValues(CONTEXT ctx)
        {
            RegisterValues.Clear();
            RegisterValues.Add(new RegisterValueEntry()
            {
                Name = "EAX",
                ValueHex = ctx.Eax.ToString("X8"),
                Backup = ctx.Eax
            });

            RegisterValues.Add(new RegisterValueEntry()
            {
                Name = "EBX",
                ValueHex = ctx.Ebx.ToString("X8"),
                Backup = ctx.Ebx
            });

            RegisterValues.Add(new RegisterValueEntry()
            {
                Name = "ECX",
                ValueHex = ctx.Ecx.ToString("X8"),
                Backup = ctx.Ecx
            });

            RegisterValues.Add(new RegisterValueEntry()
            {
                Name = "EDX",
                ValueHex = ctx.Edx.ToString("X8"),
                Backup = ctx.Edx
            });

            RegisterValues.Add(new RegisterValueEntry()
            {
                Name = "ESI",
                ValueHex = ctx.Esi.ToString("X8"),
                Backup = ctx.Esi
            });

            RegisterValues.Add(new RegisterValueEntry()
            {
                Name = "EIP",
                ValueHex = ctx.Eip.ToString("X8"),
                Backup = ctx.Eip
            });
            DebuggerDataGrid.Items.Refresh();
        }

        private void ParseRegiserValues()
        {
            foreach(var a in RegisterValues.Cast<RegisterValueEntry>())
            {
                switch(a.Name)
                {
                    case "EAX":
                        uint.TryParse(a.ValueHex, NumberStyles.HexNumber, null, out CurrentContext.Eax);
                        break;
                    case "EBX":
                        uint.TryParse(a.ValueHex, NumberStyles.HexNumber, null, out CurrentContext.Ebx);
                        break;
                    case "ECX":
                        uint.TryParse(a.ValueHex, NumberStyles.HexNumber, null, out CurrentContext.Ecx);
                        break;
                    case "EDX":
                        uint.TryParse(a.ValueHex, NumberStyles.HexNumber, null, out CurrentContext.Edx);
                        break;
                    case "ESI":
                        uint.TryParse(a.ValueHex, NumberStyles.HexNumber, null, out CurrentContext.Esi);
                        break;
                    case "EIP":
                        uint.TryParse(a.ValueHex, NumberStyles.HexNumber, null, out CurrentContext.Eip);
                        break;
                }
            }
        }

        private void DebuggerContinueButton_Click(object sender, RoutedEventArgs e)
        {
            IsExecutionPaused = false;
            IsWaitingForPause = false;
            DebuggerContinueButton.IsEnabled = false;

            ParseRegiserValues();
            TargetDebugger.SetContext(CurrentContext);
            RegisterValues.Clear();
            DebuggerDataGrid.Items.Refresh();

            var breakEntry = BreakpointDataGrid.Items.Cast<BreakpointEntry>().Where(f => ulong.Parse(f.Address, NumberStyles.HexNumber) == CurrentContext.Eip).FirstOrDefault();
            if (breakEntry != null)
            {
                breakEntry.LastTime = DateTime.Now;
                breakEntry.BreakpointHit = "";              
            }
              
            var disasmEntry = DisassemblerDataGrid.Items.Cast<DisassemblerValueEntry>().Where(f => ulong.Parse(f.Address, NumberStyles.HexNumber) == CurrentContext.Eip).FirstOrDefault();
            if (disasmEntry != null)
                disasmEntry.BreakpointHit = "";

            BreakpointDataGrid.SelectedItem = null;
            TargetDebugger.ContinueDebugEvent();
        }

        private void DebuggerAddBreakpointButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleBreakpointOnSelection();
        }

        private void DebuggerCallback(Debugger dbg, ref CONTEXT ctx)
        { 
            ulong addy = ctx.Eip;
            var breakEntry = BreakpointDataGrid.Items.Cast<BreakpointEntry>().Where(f => ulong.Parse(f.Address, NumberStyles.HexNumber) == addy).FirstOrDefault();

            if (breakEntry == null)
                return;

            ContextMutex.WaitOne();
            IsWaitingForPause = true;
            IsExecutionPaused = true;
            CurrentContext = ctx;
            ContextMutex.ReleaseMutex();
            TargetDebugger.HaltDebugEvent();       
        }

        private void ThisThreadDebuggerCallback()
        {
            ulong addy = CurrentContext.Eip;
            var breakEntry = BreakpointDataGrid.Items.Cast<BreakpointEntry>().Where(f => ulong.Parse(f.Address, NumberStyles.HexNumber) == addy).FirstOrDefault();

            if (breakEntry == null)
                return;

            var disasmEntry = DisassemblerDataGrid.Items.Cast<DisassemblerValueEntry>().Where(f => ulong.Parse(f.Address, NumberStyles.HexNumber) == addy).FirstOrDefault();

            breakEntry.Count = (int.Parse(breakEntry.Count) + 1).ToString();
            breakEntry.Time = (DateTime.Now - breakEntry.LastTime).TotalMilliseconds.ToString();

            BreakpointDataGrid.SelectedItem = breakEntry;
            DisassemblerDataGrid.SelectedItem = disasmEntry;

            breakEntry.BreakpointHit = "+";
            if (disasmEntry != null)
                disasmEntry.BreakpointHit = "+";

            IsExecutionPaused = true;
            DebuggerContinueButton.IsEnabled = true;

            UpdateRegisterValues(CurrentContext);
        }

        private void ToggleBreakpointOnSelection()
        {
            if (DisassemblerDataGrid.SelectedItem == null)
                return;

            var address = ulong.Parse(((DisassemblerValueEntry)(DisassemblerDataGrid.SelectedItem)).Address, NumberStyles.HexNumber);
            var existingItem = BreakpointDataGrid.Items.Cast<BreakpointEntry>()
                .Where(x => ulong.Parse(x.Address, NumberStyles.HexNumber) == address).FirstOrDefault();

            if (existingItem != null)
            {
                RemoveBreakpoint(address);
                return;
            }

            if (BreakpointDataGrid.Items.Count >= 4)
            {
                MessageBox.Show("Cannot add more than 4 hardware breakpoints!", "Oops!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TargetDebugger.AddBreakpoint(address, DebuggerCallback);
            BreakpointDataGrid.Items.Add(new BreakpointEntry()
            {
                Address = address.ToString("X8"),
                Count = "0",
                Time = "0",
                LastTime = DateTime.Now,
                BreakpointHit = ""
            });
        }

        private void RemoveBreakpoint(ulong address)
        {
            var existingItem = BreakpointDataGrid.Items.Cast<BreakpointEntry>()
                .Where(x => ulong.Parse(x.Address, NumberStyles.HexNumber) == address).FirstOrDefault();

            if (existingItem == null)
                return;

            BreakpointDataGrid.Items.Remove(existingItem);
            TargetDebugger.RemoveBreakpoint(address);
        }

        private void DisassemblerDataGrid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                if (DisassemblerDataGrid.SelectedItem == null)
                    return;

                new Assembler((DisassemblerEntry)DisassemblerDataGrid.SelectedItem, TargetProcess).ShowDialog();

                Update();
            }
            else if(e.Key == System.Windows.Input.Key.F2)
            {
                ToggleBreakpointOnSelection();
            }
        }

        private void BreakpointDataGrid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Back)
            {
                if (BreakpointDataGrid.SelectedItem == null)
                    return;

                RemoveBreakpoint(ulong.Parse(((BreakpointEntry)(BreakpointDataGrid.SelectedItem)).Address, NumberStyles.HexNumber));
            }
        }

        private void BreakpointDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (BreakpointDataGrid.SelectedItem != null)
                return;

            var breakEntry = BreakpointDataGrid.Items.Cast<BreakpointEntry>().Where(f => ulong.Parse(f.Address, NumberStyles.HexNumber) == CurrentContext.Eip).FirstOrDefault();
            if (breakEntry != null)
            {
                BreakpointDataGrid.SelectedItem = breakEntry;   
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartAddressTextBox.Text = TargetProcess.GetProcess().MainModule.EntryPointAddress.ToString("X8");
            SizeTextBox.Text = "1024";
            Update();

            DebuggerDataGrid.ItemsSource = RegisterValues;
            DebuggerDataGrid.Items.Refresh();
        }
    }
}
