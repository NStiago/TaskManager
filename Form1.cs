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
        private List<Items> processesWithMemory = new List<Items>();
        Thread gettingProcesses;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            itemsBindingSource.DataSource = processesWithMemory;

            //Thread gettingProcesses = new Thread(GetProcceses);
            //gettingProcesses.Start();
            //Thread fillingProcess = new Thread(FillListView);
            //fillingProcess.Start();

            gettingProcesses = new Thread(() =>
            {
                GetProcceses();
                FillListView();
            });
            gettingProcesses.Start();


            toolStripButtonStart.Enabled = false;


            //GetProcceses();
            //FillListView();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
            BeginInvoke(new Action(() =>
            {
                labelCount.Text = processList.Count.ToString();
            }));

        }

        private void FillListView()
        {


            BeginInvoke(new Action(() =>
            {
                
            }));

            //int positionIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
            //int selectedPositionIndex = dataGridView1.CurrentCell.RowIndex;

            //BeginInvoke(new Action(() =>
            //{
            //    dataGridView1.Rows.Clear();
            //}));

            lock (processList)
            {
                foreach (Process process in processList)

                {

                    //PerformanceCounter counter = new PerformanceCounter();
                    //counter.CategoryName = "Process";
                    //counter.CounterName = "Working Set - Private";
                    //counter.InstanceName = process.ProcessName;
                    //скорректировать вычисление занимаемой памяти
                    var memorySize = Convert.ToDouble(process.WorkingSet64) / (1024 * 1024);
                    //var memorySize = Convert.ToDouble(counter.NextValue()) / (1024 * 1024);
                    memorySize = Math.Round(memorySize, 2);

                    


                    Items item = new Items(process.Id, process.ProcessName.ToString(), memorySize, process.Responding == true ? "Responding" : "Not responding");
                    //var rows = new string[] { process.ProcessName.ToString(), memorySize.ToString(), process.Responding == true ? "Responding" : "Not responding" };
                    if (!itemsBindingSource.Contains(item))
                    {
                        BeginInvoke(new Action(() =>  itemsBindingSource.Add(item) ));
                    }

                    //BeginInvoke(new Action(() =>
                    //{
                    //    dataGridView1.Rows.Add(rows);
                    //}));
                    //counter.Close();
                    //counter.Dispose();
                }

                
                //BeginInvoke(new Action(() =>
                //{
                //    dataGridView1.CurrentCell = dataGridView1.Rows[selectedPositionIndex].Cells[0];
                //    dataGridView1.FirstDisplayedScrollingRowIndex = positionIndex;
                //}));
            }

        }

        private string GetInformation()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (processItemsDGV.SelectedCells != null)
                {
                    Process process = processList.Where(x => x.ProcessName == processItemsDGV.CurrentRow.Cells[0].Value.ToString()).ToList().FirstOrDefault();
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

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            GetProcceses();
            FillListView();

        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (processItemsDGV.SelectedCells != null)
                {
                    Process processToKill = processList.Where(x => x.ProcessName == processItemsDGV.CurrentRow.Cells[0].Value.ToString()).ToList().FirstOrDefault();
                    KillProcess(processToKill);
                    GetProcceses();
                    FillListView();
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

        private void dataGridView1_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}