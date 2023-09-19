using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace TaskManager
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ProcessInfo> ProcessList { get; set; }

        public MainWindow()
        {
            this.InitializeComponent();
            this.ProcessList = new ObservableCollection<ProcessInfo>();
            this.processListView.ItemsSource = this.ProcessList;
            this.ListProcesses_Click();
        }

        private void ListProcesses_Click()
        {
            var processes = Process.GetProcesses();
            this.ProcessList.Clear();

            foreach (var process in processes)
            {
                this.ProcessList.Add(new ProcessInfo
                {
                    Name = process.ProcessName,
                    Id = process.Id,
                    Priority = process.BasePriority,
                    MemoryUsage = (double)process.WorkingSet64 / (1024 * 1024) // Convert to MB
                });
            }
        }
        private void EndSelectedProcess_Click(object sender, RoutedEventArgs e)
        {
            if (this.processListView.SelectedItem != null)
            {
                var selectedProcess = (ProcessInfo)this.processListView.SelectedItem;
                try
                {
                    var processToKill = Process.GetProcessById(selectedProcess.Id);
                    processToKill.Kill();
                    this.ProcessList.Remove(selectedProcess);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to end process: {ex.Message}");
                }
            }
        }
    }

    public class ProcessInfo
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Priority { get; set; }
        public double MemoryUsage { get; set; }
    }
}
