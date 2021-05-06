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
		private ASCIIEncoding AsciiConverter = new ASCIIEncoding();

		private MemoryProcess SelectedProcess = null;
		private MemorySearcher Searcher = new MemorySearcher();
		private SearchType CurrentSearchType = SearchType.Int32;
		private bool WasSearching = false;
		private int TimerTick = 1;

		//String representation of 'SearchType' enumerator
		private string[] SearchTypes =
		{
			"Int8",
			"Int16",
			"Int32",
			"Int64",
			"Float",
			"Double",
			"ASCII String"
		};

		public MainWindow()
		{
			InitializeComponent();
			SelectedProcessLabel.Content = "";
			System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
			dispatcherTimer.Tick += WindowProcessTimer_Tick;
			dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
			dispatcherTimer.Start();

			for (int i = 0; i < SearchTypes.Length; i++)
				SearchTypeComboBox.Items.Add(SearchTypes[i]);

			SearchTypeComboBox.SelectedItem = SearchTypes[(int)CurrentSearchType];
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
			TimerTick = 1;
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

		private void UpdateInfoDataGrid(DataGrid datagrid)
        {
			bool needsRefresh = false;
			foreach (object ob in datagrid.Items)
			{
				SearchEntry en = (SearchEntry)ob;
				if (UpdateSearchEntry(en))
					needsRefresh = true;
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
				TimerTick = 6;
				MemorySearchResult res = Searcher.GetLastResult();
				foreach (UInt64 addy in res.Addresses)
				{
					SearchEntry entry = new SearchEntry() { OriginalAddress = addy, Address = addy.ToString("X8"), Value = "", ValueType = CurrentSearchType,
					ValueTypeString = SearchTypes[(int)CurrentSearchType] };
					SearchResultDataGrid.Items.Add(entry);
				}
			}

			TimerTick++;
			if (TimerTick % 7 == 0)
            {
				UpdateInfoDataGrid(SearchResultDataGrid);
				UpdateInfoDataGrid(AddressListDataGrid);
				TimerTick = 1;
			}
		}

		byte[] ParseInputToByteArray(string valueInput, SearchType currentType)
        {
			byte[] noret = new byte[0];
			if (valueInput.Length < 1)
				return noret;

			int intResult;
			switch (CurrentSearchType)
			{
				case SearchType.Int8:
					if (!int.TryParse(valueInput, out intResult))
						return noret;
					byte byteResult = (byte)(intResult & 0xFF);
					return new byte[] { byteResult };
				case SearchType.Int16:
					if (!int.TryParse(valueInput, out intResult))
						return noret;
					short shortResult = (short)(intResult & 0xFFFF);
					return BitConverter.GetBytes(shortResult);
				case SearchType.Int32:
					if (!int.TryParse(valueInput, out intResult))
						return noret;
					return BitConverter.GetBytes(intResult);
				case SearchType.Int64:
					long longResult;
					if (!long.TryParse(valueInput, out longResult))
						return noret;
					return BitConverter.GetBytes(longResult);
				case SearchType.Float:
					float floatResult;
					if (!float.TryParse(valueInput, out floatResult))
						return noret;
					return BitConverter.GetBytes(floatResult);
				case SearchType.Double:
					double doubleResult;
					if (!double.TryParse(valueInput, out doubleResult))
						return noret;
					return BitConverter.GetBytes(doubleResult);
				case SearchType.String:
					return AsciiConverter.GetBytes(valueInput);
			}

			return noret;
		}

		private void NewSearchButton_Click(object sender, RoutedEventArgs e)
		{
			SearchResultDataGrid.Items.Clear();
			SearchProgressBar.Value = 0;

			string valueInput = SearchValueTextBox.Text.Trim();
			if (valueInput.Length < 1)
				return;

			byte[] searchBytes = ParseInputToByteArray(valueInput, CurrentSearchType);
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

			byte[] searchBytes = ParseInputToByteArray(valueInput, CurrentSearchType);
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

		private void AddressDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
			if (e.Key != Key.Delete)
				return;

			if (AddressListDataGrid.SelectedIndex == -1)
				return;

			AddressListDataGrid.Items.Remove(AddressListDataGrid.SelectedItem);
        }
	}
}
