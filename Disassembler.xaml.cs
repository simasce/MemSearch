using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using SimAssembler;

namespace MemSearch
{
    /// <summary>
    /// Interaction logic for Disassembler.xaml
    /// </summary>
    public partial class Disassembler : Window
    {
        private MemoryProcess TargetProcess { get; set; }

        public Disassembler(MemoryProcess targetProcess)
        {
            TargetProcess = targetProcess;

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += WindowProcessTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
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
            UpdateButton.IsEnabled = TargetProcess.IsOpen();
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
                    var entry = new DisassemblerEntry()
                    {
                        Address = dis.Offset.ToString("X"),
                        ByteString = string.Join(' ', dis.Bytes.Select(f => f.ToString("X2"))),
                        Code = dis.Result,
                        Size = dis.Bytes.Count

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
        }
    }
}
