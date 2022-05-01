using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MemSearch
{
    /// <summary>
    /// Interaction logic for CodeInjectionWindow.xaml
    /// </summary>
    public partial class CodeInjectionWindow : Window
    {
        private readonly string _initialCode = ".code\n\n;Enter your assembly code here\n;Don't forget to jump to 'jumpBack' to continue execution after the hook!\n\noriginalCode\nJMP jumpBack;\n\n.data\n";

        private DisassemblerEntry _currentEntry;
        private MemoryProcess _memoryProcess;

        public CodeInjectionWindow(MemoryProcess curProcess, DisassemblerEntry entry)
        {
            InitializeComponent();

            _currentEntry = entry;
            _memoryProcess = curProcess;

            AddressLabel.Content = "Address: " + entry.Address;

            CodeBox.Text = _initialCode.Replace("originalCode", GetOverwrittenOpcodes());
        }

        private void InjectCodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_memoryProcess.IsOpen())
                return;

            ulong originalAddress = ulong.Parse(_currentEntry.Address, System.Globalization.NumberStyles.HexNumber);
            string toCompile = CodeBox.Text.Trim();
            var linkerPointers = new Dictionary<string, ulong>() 
            {
                {"jumpBack", (originalAddress + 5)}
            };

            bool compiled = false;
            var assembled = SimAssembler.Assembler.AssembleWithExtraLinkers(toCompile, out compiled, originalAddress, linkerPointers); //will need to be recompiled with correct base address after allocating
            if (!compiled || assembled.Count == 0)
            {
                MessageBox.Show("Could not compile the given code!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            uint size = (uint)assembled.Sum(f => f.Bytes.Count);

            ulong address = _memoryProcess.AllocateMemory(size, MemoryProtection.PAGE_EXECUTE_READWRITE);
            if(address == 0)
            {
                MessageBox.Show("Failed to allocate memory for code injection!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            assembled = SimAssembler.Assembler.AssembleWithExtraLinkers(toCompile, out compiled, address, linkerPointers); //recompile with correct base
            if (!compiled || assembled.Count == 0)
            {
                MessageBox.Show("Could not compile the given code!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _memoryProcess.WriteBuffer(address, assembled.SelectMany(f => f.Bytes).ToArray());

            UInt32 jmpPtr = (UInt32)address - (UInt32)(originalAddress + 5);
            var outputBytes = new byte[] { 0xE9 }.Concat(BitConverter.GetBytes(jmpPtr)).ToArray();

            int fullsize = outputBytes.Length;
            int origSize = _currentEntry.Size;
            int numNOPS = Math.Max(0, origSize - fullsize);

            if (fullsize > origSize)
            {
                bool finished = false; //unused
                var disassemble = SimAssembler.Assembler.Disassemble(_memoryProcess.ReadBuffer(originalAddress, 32), out finished, originalAddress);

                int calculated = 0;
                var toBeReplaced = new List<SimAssembler.OpcodeReturnInfo>();
                foreach (var z in disassemble)
                {
                    toBeReplaced.Add(z);
                    calculated += z.Bytes.Count;
                    if (calculated >= fullsize)
                        break;
                }
                numNOPS = calculated - fullsize;
            }

            _memoryProcess.WriteBuffer(originalAddress, outputBytes.Concat(Enumerable.Repeat<byte>(0x90, numNOPS)).ToArray());

            this.Close();
        }

        private string GetOverwrittenOpcodes()
        {
            int fullsize = 5;
            int origSize = _currentEntry.Size;
            ulong originalAddress = ulong.Parse(_currentEntry.Address, System.Globalization.NumberStyles.HexNumber);

            if (fullsize > origSize)
            {
                bool finished = false; //unused
                var disassemble = SimAssembler.Assembler.Disassemble(_memoryProcess.ReadBuffer(originalAddress, 32), out finished, originalAddress);

                int calculated = 0;
                var toBeReplaced = new List<SimAssembler.OpcodeReturnInfo>();
                foreach (var z in disassemble)
                {
                    toBeReplaced.Add(z);
                    calculated += z.Bytes.Count;
                    if (calculated >= fullsize)
                        break;
                }

                return string.Join("\n", toBeReplaced.Select(f => f.Result + ";"));
            }
            return _currentEntry.Code + ";\n";
        }
    }
}
