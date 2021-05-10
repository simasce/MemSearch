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
    /// Interaction logic for EditValueWindow.xaml
    /// </summary>
    public partial class EditValueWindow : Window
    {
        private SearchEntry TargetEntry;
        private MemoryProcess TargetProcess;

        public EditValueWindow(SearchEntry editEntry, MemoryProcess targetProcess)
        {
            TargetEntry = editEntry;
            TargetProcess = targetProcess;

            InitializeComponent();

            ValueLabel.Content = "Value ("+editEntry.ValueTypeString+")";
            ValueTextBox.Text = editEntry.Value;
            FrozenCheckBox.IsChecked = editEntry.Frozen;
        }

        void SetEntry()
        {
            if (ValueTextBox.Text.Trim().Length <= 0)
                return;

            byte[] writeBuf = SearchTypeConverter.ParseInputToByteArray(ValueTextBox.Text, TargetEntry.ValueType);
            if (writeBuf.Length <= 0)
            {
                MessageBox.Show("Failed to convert input value!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (TargetEntry.ValueType == SearchType.String)
            {
                List<byte> strList = writeBuf.ToList();
                strList.Add((byte)0x00);
                writeBuf = strList.ToArray();
            }

            TargetEntry.Frozen = FrozenCheckBox.IsChecked.Value;
            TargetEntry.Value = ValueTextBox.Text;
            
            try
            {
                TargetProcess.WriteBuffer(TargetEntry.OriginalAddress, writeBuf);
            }
            catch(Exception e)
            {

            }

            this.Close();
        }

        private void SetButton_Click(object sender, RoutedEventArgs e)
        {
            SetEntry();
        }
    }
}
