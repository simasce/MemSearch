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
    /// Interaction logic for Assembler.xaml
    /// </summary>
    public partial class Assembler : Window
    {
        private MemoryProcess _memoryProcess;
        private DisassemblerEntry _currentEntry;

        public Assembler(DisassemblerEntry entry, MemoryProcess proc)
        {
            InitializeComponent();            

            _currentEntry = entry;
            _memoryProcess = proc;

            AssemblerBox.Text = _currentEntry.Code;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Assemble();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                Assemble();
        }

        private void Assemble()
        {
            if (!_memoryProcess.IsOpen())
                return;

            string toAssemble = ".code\n" + AssemblerBox.Text.Trim();
            bool compiled = false;
            ulong targetAddress = ulong.Parse(_currentEntry.Address, System.Globalization.NumberStyles.HexNumber);

            var assembled = SimAssembler.Assembler.Assemble(toAssemble, out compiled, targetAddress);
            if(!compiled || assembled.Count == 0)
            {
                MessageBox.Show("Could not compile the given code!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(assembled.Count > 1)
            {
                MessageBox.Show("You can only compile a single opcode!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int fullsize = assembled.Sum(f => f.Bytes.Count);
            int origSize = _currentEntry.Size;
            int numNOPS = Math.Max(0, origSize - fullsize);

            if (fullsize > origSize)
            {
                bool finished = false; //unused
                var disassemble = SimAssembler.Assembler.Disassemble(_memoryProcess.ReadBuffer(targetAddress, 32), out finished, targetAddress);

                int calculated = 0;
                var toBeReplaced = new List<SimAssembler.OpcodeReturnInfo>();
                foreach(var e in disassemble)
                {
                    toBeReplaced.Add(e);
                    calculated += e.Bytes.Count;
                    if (calculated >= fullsize)
                        break;
                }
                numNOPS = calculated - fullsize;
            }

            _memoryProcess.WriteBuffer(targetAddress, assembled.SelectMany(f => f.Bytes)
                .Concat(Enumerable.Repeat<byte>(0x90, numNOPS))
                .ToArray());

            this.Close();
        }
    }
}
