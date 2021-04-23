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
using System.Windows.Shapes;

namespace MemSearch
{
	public class ProcessInfo
	{
		public string ProcessName { get; set; }
		public string ProcessID { get; set; }
		public Process ThisProcess { get; set; }
	}
	public partial class ProcessSelect : Window
	{
		private MainWindow ProcessMainWindow;
		private Process[] AvailableProcesses;
		public ProcessSelect(MainWindow refWindow)
		{
			ProcessMainWindow = refWindow;
			InitializeComponent();
			ProcessSelectButton.IsEnabled = false;
			AvailableProcesses = Process.GetProcesses();
			for(int i = 0; i < AvailableProcesses.Length; i++)
			{
				ProcessSelectDataGrid.Items.Add(new ProcessInfo() { ProcessName = AvailableProcesses[i].ProcessName, 
					ProcessID = AvailableProcesses[i].Id.ToString(), ThisProcess = AvailableProcesses[i]});
			}
		}

		private void SelectProcess()
		{
			if (ProcessSelectDataGrid.SelectedItems.Count < 1)
				return;

			ProcessInfo selectedProc = (ProcessInfo)ProcessSelectDataGrid.SelectedItem;
			try
			{
				MemoryProcess memProc = new MemoryProcess(selectedProc.ThisProcess);
				ProcessMainWindow.SetSelectedProcess(memProc);
				this.Close();
			}
			catch (Exception e)
			{
				MessageBox.Show("Error: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}				   		
		}

		private void ProcessSelectButton_Click(object sender, RoutedEventArgs e)
		{
			SelectProcess();
		}

		private void ProcessSelectDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ProcessSelectButton.IsEnabled = true;
		}

		private void ProcessSelectDataGridRow_DoubleClick(object sender, EventArgs e)
		{
			SelectProcess();
		}
	}
}
