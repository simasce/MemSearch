using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MemSearch
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>

	public partial class MainWindow : Window
	{
		private MemoryProcess SelectedProcess = null;
		private MemorySearcher Searcher = new MemorySearcher();
		private SearchDataLoader SearchLoader = new SearchDataLoader();

		private SearchType CurrentSearchType = SearchType.Int32;
		private bool WasSearching = false;
		private int TimerTick = 1;

        public MainWindow()
		{
			InitializeComponent();
			SelectedProcessLabel.Content = "";
			System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
			dispatcherTimer.Tick += WindowProcessTimer_Tick;
			dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
			dispatcherTimer.Start();

			for (int i = 0; i < SearchTypeConverter.SearchTypeStrings.Length; i++)
				SearchTypeComboBox.Items.Add(SearchTypeConverter.SearchTypeStrings[i]);

			SearchTypeComboBox.SelectedItem = SearchTypeConverter.SearchTypeStrings[(int)CurrentSearchType];
			SearchProgressBar.Maximum = 100;
			SearchProgressBar.Minimum = 0;
        }

		private bool ProcessSelected()
		{
			if (SelectedProcess == null)
				return false;

			return SelectedProcess.IsOpen();
		}

		public void SetSelectedProcess(MemoryProcess memProc)
		{
			if(!memProc.IsOpen())
			{
				MessageBox.Show("Selected process is closed or inaccessible!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			SelectedProcess = memProc;
			SelectedProcessLabel.Content = memProc.GetProcess().ProcessName;
			Searcher.SelectProcess(memProc);
			EnableControls(true);
		}

		private void SelectProcessButton_Click(object sender, RoutedEventArgs e)
		{
			ProcessSelect selectProcWindow = new ProcessSelect(this);
			selectProcWindow.ShowDialog();
		}

		private void EnableControls(bool enable)
		{
			SearchValueTextBox.IsEnabled = enable;
			SearchTypeComboBox.IsEnabled = enable;
			SearchValueTextBox.IsEnabled = enable;
			SearchNextButton.IsEnabled = enable;
			NewSearchButton.IsEnabled = enable;
			AddressListDataGrid.IsEnabled = enable;
			SearchResultDataGrid.IsEnabled = enable;
			SaveButton.IsEnabled = enable;
			LoadButton.IsEnabled = enable;
            TimerTick = 1;
            UpdateDisassemblerButton(enable);
        }

        private void UpdateDisassemblerButton(bool enable)
        {
			if(SelectedProcess != null && SelectedProcess.Is64Bit())
                DisassemblerButton.IsEnabled = false;
			else
			    DisassemblerButton.IsEnabled = enable;
		}

		private bool UpdateSearchEntry(SearchEntry en)
        {
			string outValue = en.Value;
			switch (en.ValueType)
			{
				case SearchType.Int8:
					outValue = SelectedProcess.ReadByte(en.OriginalAddress).ToString();
					break;
				case SearchType.Int16:
					outValue = SelectedProcess.ReadInt16(en.OriginalAddress).ToString();
					break;
				case SearchType.Int32:
					outValue = SelectedProcess.ReadInt32(en.OriginalAddress).ToString();
					break;
				case SearchType.Int64:
					outValue = SelectedProcess.ReadInt64(en.OriginalAddress).ToString();
					break;
				case SearchType.Float:
					outValue = SelectedProcess.ReadFloat(en.OriginalAddress).ToString();
					break;
				case SearchType.Double:
					outValue = SelectedProcess.ReadDouble(en.OriginalAddress).ToString();
					break;
				case SearchType.String:
					outValue = SelectedProcess.ReadStringToEnd(en.OriginalAddress);
					break;
			}
			bool ret = outValue != en.Value;
			en.Value = outValue;
			return ret;
		}

		private void UpdateSearchEntryFrozen(SearchEntry en)
        {
			byte[] write = SearchTypeConverter.ParseInputToByteArray(en.Value, en.ValueType);
			if (write.Length <= 0)
				return; //failed to parse, no messagebox to avoid spam

			if (en.ValueType == SearchType.String)
			{
				List<byte> strList = write.ToList();
				strList.Add((byte)0x00);
				write = strList.ToArray();
			}

			SelectedProcess.WriteBuffer(en.OriginalAddress, write);
		}

		private void UpdateInfoDataGrid(DataGrid datagrid)
        {
			bool needsRefresh = false;
			foreach (object ob in datagrid.Items)
			{
				SearchEntry en = (SearchEntry)ob;
				try
                {
					if (en.Frozen)
					{
						UpdateSearchEntryFrozen(en);
					}
					else
					{
						if (UpdateSearchEntry(en))
							needsRefresh = true;
					}
				}
				catch(Exception e)
                {
					//Log?
                }
			}
			if(needsRefresh)
				datagrid.Items.Refresh();
		}

		private void WindowProcessTimer_Tick(object sender, EventArgs e)
		{
			if (!ProcessSelected())
			{
				EnableControls(false);
				return;
			}          

			if (Searcher.IsSearching())
			{
				SearchProgressBar.Value = (double)Searcher.GetSearchPercentage() * 100.0;
				EnableControls(false);
				WasSearching = true;
				return;
			}

			if(WasSearching)
            {
				EnableControls(true);
				SearchProgressBar.Value = 100;
				SearchResultDataGrid.Items.Clear();
				WasSearching = false;
				TimerTick = 4;
				MemorySearchResult res = Searcher.GetLastResult();
				foreach (UInt64 addy in res.Addresses)
				{
					SearchEntry entry = new SearchEntry() { OriginalAddress = addy, Address = addy.ToString("X8"), Value = "", ValueType = CurrentSearchType, Frozen = false,
					ValueTypeString = SearchTypeConverter.SearchTypeStrings[(int)CurrentSearchType] };
					SearchResultDataGrid.Items.Add(entry);
				}
			}

			TimerTick++;
			if (TimerTick % 5 == 0)
            {
				UpdateInfoDataGrid(SearchResultDataGrid);			
				TimerTick = 1;
			}
			UpdateInfoDataGrid(AddressListDataGrid);
		}

		private void NewSearchButton_Click(object sender, RoutedEventArgs e)
		{
			SearchResultDataGrid.Items.Clear();
			SearchProgressBar.Value = 0;

			string valueInput = SearchValueTextBox.Text.Trim();
			if (valueInput.Length < 1)
				return;

			byte[] searchBytes = SearchTypeConverter.ParseInputToByteArray(valueInput, CurrentSearchType);
			if(searchBytes.Length <= 0)
            {
				MessageBox.Show("Failed to convert input value to selected type!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
            }

			Searcher.SearchByteArray(searchBytes);
			EnableControls(false);
			WasSearching = true;
		}

		private void SearchTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			CurrentSearchType = (SearchType)SearchTypeComboBox.SelectedIndex;
		}

        private void SearchNextButton_Click(object sender, RoutedEventArgs e)
        {
			SearchResultDataGrid.Items.Clear();
			SearchProgressBar.Value = 0;

			string valueInput = SearchValueTextBox.Text.Trim();
			if (valueInput.Length < 1)
				return;

			byte[] searchBytes = SearchTypeConverter.ParseInputToByteArray(valueInput, CurrentSearchType);
			if (searchBytes.Length <= 0)
			{
				MessageBox.Show("Failed to convert input value to selected type!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			Searcher.SearchAgain(searchBytes);
			EnableControls(false);
			WasSearching = true;
		}

		private void SearchResultDataGridRow_DoubleClick(object sender, EventArgs e)
		{
			if (SearchResultDataGrid.SelectedItems.Count < 1)
				return;

			SearchEntry entry = (SearchEntry)SearchResultDataGrid.SelectedItem;

			foreach(SearchEntry se in AddressListDataGrid.Items)
            {
				if (entry.Address == se.Address)
					return; //already exists
            }
			AddressListDataGrid.Items.Add(entry);
		}

		private void AddressListDataGridRow_DoubleClick(object sender, EventArgs e)
		{
			if (AddressListDataGrid.SelectedItems.Count < 1)
				return;

			if (!ProcessSelected())
				return;

			SearchEntry entry = (SearchEntry)AddressListDataGrid.SelectedItem;
			EditValueWindow valWindow = new EditValueWindow(entry, SelectedProcess);
			valWindow.ShowDialog();
			AddressListDataGrid.Items.Refresh();
		}

		private void AddressDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
			if (e.Key != Key.Delete)
				return;

			if (AddressListDataGrid.SelectedIndex == -1)
				return;

			AddressListDataGrid.Items.Remove(AddressListDataGrid.SelectedItem);
        }

		private void SaveButton_Click(object sender, EventArgs e)
        {
			if(AddressListDataGrid.Items.Count <= 0)
            {
				MessageBox.Show("There isn't any data in the address box!", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
            }

			SaveFileDialog saveDialog = new SaveFileDialog();

			saveDialog.Filter = "MemSearch Binaries (*.msearch)|*.msearch|All files (*.*)|*.*";
			saveDialog.FilterIndex = 1;
			saveDialog.RestoreDirectory = true;

			if (!saveDialog.ShowDialog().Value)
				return;

			List<SearchEntry> saveList = new List<SearchEntry>();
			foreach(SearchEntry a in AddressListDataGrid.Items)
				saveList.Add(a);

			SearchLoader.SaveFile(saveDialog.FileName, saveList, SelectedProcess.GetProcess().ProcessName);
		}

		private void LoadButton_Click(object sender, EventArgs e)
		{
			if (AddressListDataGrid.Items.Count > 0)
			{
				MessageBoxResult res = MessageBox.Show("Loading a saved file will clear the address box!\nDo you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				if(res == MessageBoxResult.No)
					return;
			}

			OpenFileDialog loadDialog = new OpenFileDialog();

			loadDialog.Filter = "MemSearch Binaries (*.msearch)|*.msearch|All files (*.*)|*.*";
			loadDialog.FilterIndex = 1;
			loadDialog.RestoreDirectory = true;

			if (!loadDialog.ShowDialog().Value)
				return;

			List<SearchEntry> loadList;
			string loadProcName;

			if (!SearchLoader.LoadFile(loadDialog.FileName, out loadProcName, out loadList))
				return; //failed to load file

			if(loadProcName != SelectedProcess.GetProcess().ProcessName)
            {
				MessageBoxResult res = MessageBox.Show("Current loaded process name mismatches the name written to saved file.\nDo you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				if (res == MessageBoxResult.No)
					return;
			}

			AddressListDataGrid.Items.Clear();
			foreach (SearchEntry entry in loadList)
				AddressListDataGrid.Items.Add(entry);
			AddressListDataGrid.Items.Refresh();
		}

        private void DisassemblerButton_Click(object sender, RoutedEventArgs e)
        {
			OpenDisassemblerWindow();
        }

        private void OpenDisassemblerWindow()
        {
            if (SelectedProcess == null || SelectedProcess.Is64Bit() || !SelectedProcess.IsOpen())
                return;

            Disassembler ds = new Disassembler(SelectedProcess);
            ds.Show();
        }
    }
}
