using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Management;
using System.Text;

namespace TaskManager__Businescope_
{
    public partial class Form1 : Form
    {
        private List<Process> processList = new List<Process>();
        Thread gettingProcesses;

        List<ProcessForDisplaying> processToDisplay = new List<ProcessForDisplaying>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gettingProcesses = new Thread(() =>
            {
                GetProcceses();
                FillListView();
                GetCountOfProcess(processList);

            });
            gettingProcesses.Start();
            toolStripButtonStart.Enabled = false;
        }

        private void toolStripButtonStart_Click(object sender, EventArgs e)
        {
            toolStripButtonStop.Enabled = true;
            toolStripButtonStart.Enabled = false;
            timer2.Enabled = true;
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            toolStripButtonStart.Enabled = true;
            toolStripButtonStop.Enabled = false;
            timer2.Enabled = false;
        }

        private void GetProcceses()
        {
            lock (processList)
            {
                processList.Clear();
                processList = Process.GetProcesses().OrderBy(x => x.ProcessName).ToList();
            }
        }

        private void GetCountOfProcess(List<Process> processList)
        {
            BeginInvoke(new Action(() =>
            {
                labelCount.Text = processList.Count.ToString();
            }));
        }

        private void FillListView()
        {
            int positionIndex = processGridView.FirstDisplayedScrollingRowIndex;
            int selectedPositionIndex = processGridView.CurrentCell.RowIndex;
            BeginInvoke(new Action(() =>processGridView.Rows.Clear()));
            lock (processList)
            {
                foreach (Process process in processList)

                {
                    //Рассмотреть вариант с использованием PerfomanceCounter:
                    //PerformanceCounter counter = new PerformanceCounter();
                    //counter.CategoryName = "Process";
                    //counter.CounterName = "Working Set - Private";
                    //counter.InstanceName = process.ProcessName;
                    var memorySize = Convert.ToDouble(process.WorkingSet64) / (1024 * 1024);
                    //var memorySize = Convert.ToDouble(counter.NextValue()) / (1024 * 1024);
                    memorySize = Math.Round(memorySize, 2);
                    ProcessForDisplaying item = new ProcessForDisplaying(
                        process.Id,
                        process.ProcessName.ToString(),
                        memorySize,
                        process.Responding == true ? "Responding" : "Not responding");
                    BeginInvoke(new Action(() => processGridView.Rows.Add(item.GetRow())));
                    //counter.Close();
                    //counter.Dispose();
                }
                BeginInvoke(new Action(() =>
                {
                    processGridView.CurrentCell = processGridView.Rows[selectedPositionIndex].Cells[0];
                    processGridView.FirstDisplayedScrollingRowIndex = positionIndex;
                }));
            }
        }

        private string GetInformation()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (processGridView.SelectedCells != null)
                {
                    Process process = processList.Where(x => x.Id.ToString() == processGridView.CurrentRow.Cells[0].Value.ToString()).ToList().FirstOrDefault();
                    sb.Append($"ID процесса: {process.Id}\n");
                    sb.Append($"Название процесса: {process.ProcessName}\n");
                    sb.Append($"Время запуска: {process.StartTime}\n");
                    sb.Append(process.Responding == true ? "Состояние: в работе\n" : "Состояние: не отвечает\n");
                    sb.Append($"Handle: {process.Handle}\n");
                    sb.Append($"Путь: {process.MainModule.FileName}\n");
                }
            }
            catch (Exception) { }
            return sb.ToString();
        }
        private void KillProcess(Process process)
        {
            process.Kill();
            process.WaitForExit();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {

            GetProcceses();
            FillListView();
            GetCountOfProcess(processList);
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (processGridView.SelectedCells != null)
                {
                    Process processToKill = processList.Where(x => x.Id.ToString() == processGridView.CurrentRow.Cells[0].Value.ToString()).ToList().FirstOrDefault();
                    KillProcess(processToKill);
                    GetProcceses();
                    FillListView();
                    GetCountOfProcess(processList);
                }
            }
            catch (Exception) { }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void получитьИнформациюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetInformation());
        }
    }
}