using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Management;
using System.Text;
using System.Windows.Forms;
using TaskManager__Businescope_.Models;

namespace TaskManager__Businescope_
{
    public partial class Form1 : Form
    {
        List<Process> processList = new List<Process>();
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
                lock (processList)
                {
                    processList.Clear();
                    processList = BasicActions.ProccesData.GetProcessList();

                    processToDisplay.Clear();
                    processToDisplay = BasicActions.ProccesData.GetProcessForDisplayingList(processList);

                    BeginInvoke(new Action(() => processDataGridView.DataSource = BasicActions.ProccesData.serializeProcessForDisplaying(processToDisplay)));
                    BeginInvoke(new Action(() => labelCount.Text = BasicActions.ProccesData.GetCountOfProcess(processList)));
                }
            });
            gettingProcesses.Start();
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

        private string GetInformation()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (processDataGridView.SelectedCells != null)
                {
                    Process process = processList.Where(x => x.Id.ToString() == processDataGridView.CurrentRow.Cells[0].Value.ToString()).ToList().FirstOrDefault();

                    sb.Append($"ID процесса: {process.Id}\n");
                    sb.Append($"Название процесса: {process.ProcessName}\n");
                    sb.Append(process.Responding == true ? "Состояние: в работе\n" : "Состояние: не отвечает\n");
                    sb.Append($"Время запуска: {process.StartTime}\n");
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
            int positionIndex = 0;
            int selectedPositionIndex = 0;
            if (processDataGridView.FirstDisplayedScrollingRowIndex != -1)
                positionIndex = processDataGridView.FirstDisplayedScrollingRowIndex;
            if (processDataGridView.CurrentCell != null)
                selectedPositionIndex = processDataGridView.CurrentCell.RowIndex;
            if (!gettingProcesses.IsAlive)
            {
                gettingProcesses = new Thread(() =>
                {
                    lock (processToDisplay)
                    {
                        processList.Clear();
                        processList = BasicActions.ProccesData.GetProcessList();

                        processToDisplay.Clear();
                        processToDisplay = BasicActions.ProccesData.GetProcessForDisplayingList(processList);
                    }
                });
                gettingProcesses.IsBackground = true;
                gettingProcesses.Start();
            }
            lock (processToDisplay)
            {
                processDataGridView.DataSource = BasicActions.ProccesData.serializeProcessForDisplaying(processToDisplay);
                labelCount.Text = BasicActions.ProccesData.GetCountOfProcess(processList);
            }
            processDataGridView.CurrentCell = processDataGridView.Rows[selectedPositionIndex].Cells[0];
            processDataGridView.FirstDisplayedScrollingRowIndex = positionIndex;

        }


        private void timer2_Tick(object sender, EventArgs e)
        {
            int positionIndex = 0;
            int selectedPositionIndex = 0;
            if (processDataGridView.FirstDisplayedScrollingRowIndex != -1)
                positionIndex = processDataGridView.FirstDisplayedScrollingRowIndex;
            if (processDataGridView.CurrentCell != null)
                selectedPositionIndex = processDataGridView.CurrentCell.RowIndex;
            if (!gettingProcesses.IsAlive)
            {
                gettingProcesses = new Thread(() =>
                {
                    lock (processToDisplay)
                    {
                        processList.Clear();
                        processList = BasicActions.ProccesData.GetProcessList();

                        processToDisplay.Clear();
                        processToDisplay = BasicActions.ProccesData.GetProcessForDisplayingList(processList);
                    }
                });
                gettingProcesses.IsBackground = true;
                gettingProcesses.Start();
            }
            lock (processToDisplay)
            {
                processDataGridView.DataSource = BasicActions.ProccesData.serializeProcessForDisplaying(processToDisplay);
                labelCount.Text = BasicActions.ProccesData.GetCountOfProcess(processList);
            }
            processDataGridView.CurrentCell = processDataGridView.Rows[selectedPositionIndex].Cells[0];
            processDataGridView.FirstDisplayedScrollingRowIndex = positionIndex;
        }



        private void processDataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hit = processDataGridView.HitTest(e.X, e.Y);
                if (hit.RowIndex >= 0)
                {
                    processDataGridView.ClearSelection();
                    processDataGridView.Rows[hit.RowIndex].Selected = true;
                    contextMenuStrip1.Show(processDataGridView, e.Location);
                    processDataGridView.CurrentCell = processDataGridView.Rows[hit.RowIndex].Cells[0];
                }
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetInformation());
        }

        private void killToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (processDataGridView.SelectedCells != null)
                {
                    Process processToKill = processList.Where(x => x.Id.ToString() == processDataGridView.CurrentRow.Cells[0].Value.ToString()).ToList().FirstOrDefault();
                    KillProcess(processToKill);
                    //надо будет запустить поток




                }
            }
            catch (Exception) { }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {


        }
    }
}

