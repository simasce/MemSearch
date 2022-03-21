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
            string toAssemble = ".code\n" + AssemblerBox.Text.Trim();
            bool compiled = false;
            ulong targetAddress = ulong.Parse(_currentEntry.Address, System.Globalization.NumberStyles.HexNumber);

            var assembled = SimAssembler.Assembler.Assemble(toAssemble, out compiled, targetAddress);
            if(!compiled)
            {
                MessageBox.Show("Could not compile the given code!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int fullsize = assembled.Sum(f => f.Bytes.Count);
            int origSize = _currentEntry.Size;         

            if (fullsize > origSize)
            {
                MessageBox.Show("New code is bigger than old one!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _memoryProcess.WriteBuffer(targetAddress, assembled.SelectMany(f => f.Bytes)
                .Concat(Enumerable.Repeat<byte>(0x90, origSize - fullsize))
                .ToArray());

            this.Close();
        }
    }
}
