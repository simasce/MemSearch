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

		private void UpdateInfoDataGrid(DataGrid datagrid)
        {
			foreach (object ob in datagrid.Items)
			{
				SearchEntry en = (SearchEntry)ob;

				switch (en.ValueType)
				{
					case SearchType.Int8:
						en.Value = SelectedProcess.ReadByte(en.OriginalAddress).ToString();
						break;
					case SearchType.Int16:
						en.Value = SelectedProcess.ReadInt16(en.OriginalAddress).ToString();
						break;
					case SearchType.Int32:
						en.Value = SelectedProcess.ReadInt32(en.OriginalAddress).ToString();
						break;
					case SearchType.Int64:
						en.Value = SelectedProcess.ReadInt64(en.OriginalAddress).ToString();
						break;
					case SearchType.Float:
						en.Value = SelectedProcess.ReadFloat(en.OriginalAddress).ToString();
						break;
					case SearchType.Double:
						en.Value = SelectedProcess.ReadDouble(en.OriginalAddress).ToString();
						break;
					case SearchType.String:
						en.Value = SelectedProcess.ReadStringToEnd(en.OriginalAddress);
						break;
				}
			}
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
			else
			{
				EnableControls(true);
			}

			if(WasSearching)
            {
				SearchResultDataGrid.Items.Clear();
				WasSearching = false;
				TimerTick = 10;
				MemorySearchResult res = Searcher.GetLastResult();
				foreach (UInt64 addy in res.Addresses)
				{
					SearchEntry entry = new SearchEntry() { OriginalAddress = addy, Address = addy.ToString("X8"), Value = "", ValueType = CurrentSearchType,
					ValueTypeString = SearchTypes[(int)CurrentSearchType] };
					SearchResultDataGrid.Items.Add(entry);
				}
			}

			if(TimerTick++ % 10 == 0)
            {
				UpdateInfoDataGrid(SearchResultDataGrid);
				UpdateInfoDataGrid(AddressListDataGrid);
				TimerTick = 1;
			}
		}

		private void NewSearchButton_Click(object sender, RoutedEventArgs e)
		{
			SearchResultDataGrid.Items.Clear();
			SearchProgressBar.Value = 0;

			string valueInput = SearchValueTextBox.Text.Trim();
			if (valueInput.Length < 1)
				return;

			int intResult;	
			switch(CurrentSearchType)
			{
				case SearchType.Int8:
					if (!int.TryParse(valueInput, out intResult))
						return;
					byte byteResult = (byte)(intResult & 0xFF);
					Searcher.SearchByte(byteResult);
					break;
				case SearchType.Int16:
					if (!int.TryParse(valueInput, out intResult))
						return;
					short shortResult = (short)(intResult & 0xFFFF);
					Searcher.SearchInt16(shortResult);
					break;
				case SearchType.Int32:
					if (!int.TryParse(valueInput, out intResult))
						return;
					Searcher.SearchInt32(intResult);
					break;
				case SearchType.Int64:
					long longResult;
					if (!long.TryParse(valueInput, out longResult))
						return;
					Searcher.SearchInt64(longResult);
					break;
				case SearchType.Float:
					float floatResult;
					if (!float.TryParse(valueInput, out floatResult))
						return;
					Searcher.SearchFloat(floatResult);
					break;
				case SearchType.Double:
					double doubleResult;
					if (!double.TryParse(valueInput, out doubleResult))
						return;
					Searcher.SearchDouble(doubleResult);
					break;
				case SearchType.String:
					Searcher.SearchString(valueInput);
					break;
			}
		}

		private void SearchTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			CurrentSearchType = (SearchType)SearchTypeComboBox.SelectedIndex;
		}

        private void SearchNextButton_Click(object sender, RoutedEventArgs e)
        {
			Searcher.SearchAgain();
        }

		private void SearchResultDataGridRow_DoubleClick(object sender, EventArgs e)
		{
			if (SearchResultDataGrid.SelectedItems.Count < 1)
				return;

			SearchEntry entry = (SearchEntry)SearchResultDataGrid.SelectedItem;
			AddressListDataGrid.Items.Add(entry);
		}
	}
}
