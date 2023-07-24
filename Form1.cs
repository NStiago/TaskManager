using NLog;
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
    //��������� ���������� �� ������
    public partial class Form1 : Form
    {
        //���� ���������
        List<Process> processList = new List<Process>();

        //��������� �����, � ������� ���������� ��������� ���������� �� ���������
        Thread? gettingProcesses;

        //���� ��������� ��� ����������� �� ������� �����
        List<ProcessForDisplaying> processToDisplay = new List<ProcessForDisplaying>();

        //��������������� ���� ��� ��������� ��������� ����������� ��������� (���������, ��������).
        //���������� ������ ����������� ��������� - �����������
        List<ProcessForDisplaying> processToDisplayBeforeUpdate = new List<ProcessForDisplaying>();

        //����������� ���������� � ������� NLog
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //������� ��� ��������� ���������� (� ����������� �� ������� �������, ���������� ����������� �����)
        delegate List<Process> GetProcesses();
        GetProcesses GetProcessesWithSort = BasicActions.ProccesData.GetSortetProcessListByMemory;

        public Form1()
        {
            InitializeComponent();
        }

        //��� �������� ��������� �����, ��������� ����� �����, � ������� ���������� ������������� ����������� ���������
        //����������� processDataGridView
        //�������� ����������� ����� BindingSource - �� ������ ����� ������ � ���������� ������ � �����. ����� ��� ��������� ����
        private void Form1_Load(object sender, EventArgs e)
        {
            gettingProcesses = new Thread(() =>
            {
                lock (processList)
                {
                    processList.Clear();
                    processList = GetProcessesWithSort();

                    processToDisplay.Clear();
                    processToDisplay = BasicActions.ProccesData.GetProcessForDisplayingList(processList);

                    BeginInvoke(new Action(() => 
                    {
                        processDataGridView.DataSource = new DataView(BasicActions.ProccesData.serializeProcessForDisplaying(processToDisplay));
                        for(int i=0;i< processDataGridView.Columns.Count;i++)
                           processDataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        labelCount.Text = BasicActions.ProccesData.GetCountOfProcess(processList);
                    }));
                }
            });
            gettingProcesses.Start();
            logger.Info("���������� ��������");
        }

        //����� ������� ��� ���������� ������ � �����
        private void timer2_Tick(object sender, EventArgs e)
        {
            //positionIndex � selectedPositionIndex - ��� ����������� ���������� ������ � ������� ������
            int positionIndex = 0;
            int selectedPositionIndex = 0;
            if (processDataGridView.FirstDisplayedScrollingRowIndex != -1)
                positionIndex = processDataGridView.FirstDisplayedScrollingRowIndex;
            if (processDataGridView.CurrentCell != null)
                selectedPositionIndex = processDataGridView.CurrentCell.RowIndex;

            //��������� ������ �������� �� �������
            if (!gettingProcesses.IsAlive)
            {
                gettingProcesses = new Thread(() =>
                {
                    lock (processToDisplay)
                    {
                        processList.Clear();
                        processList = GetProcessesWithSort();

                        processToDisplayBeforeUpdate.Clear();
                        processToDisplayBeforeUpdate.AddRange(processToDisplay);

                        processToDisplay.Clear();
                        processToDisplay = BasicActions.ProccesData.GetProcessForDisplayingList(processList);

                        //��������� �������� ����������� ���������. ����� ��� �������� ����, ��������� �� ������ ����������� ����������
                        foreach (var process in processToDisplay)
                            if (!processToDisplayBeforeUpdate.Any(x => x.ProcessId == process.ProcessId))
                                logger.Info($"� ������� ������� ������� {process.ProcessName} c ID: {process.ProcessId}");

                        foreach (var process in processToDisplayBeforeUpdate)
                            if (!processToDisplay.Any(x => x.ProcessId == process.ProcessId))
                                logger.Info($"� ������� ���������� ������� {process.ProcessName} c ID: {process.ProcessId}");
                    }
                });

                gettingProcesses.IsBackground = true;
                gettingProcesses.Start();
            }
            lock (processToDisplay)
            {
                processDataGridView.DataSource = new DataView(BasicActions.ProccesData.serializeProcessForDisplaying(processToDisplay));
                labelCount.Text = BasicActions.ProccesData.GetCountOfProcess(processList);
            }
            processDataGridView.CurrentCell = processDataGridView.Rows[selectedPositionIndex].Cells[0];
            processDataGridView.FirstDisplayedScrollingRowIndex = positionIndex;
        }

        //������ ��� ����������� ������ ��������� ���������� �� ���������
        private void toolStripButtonStart_Click(object sender, EventArgs e)
        {
            toolStripButtonStop.Enabled = true;
            toolStripButtonStart.Enabled = false;
            timer2.Enabled = true;
            logger.Info("������������� �������� ��������� ���������� � ���������");
        }

        //������ ��� ������������ ������ ��������� ���������� �� ���������
        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            toolStripButtonStart.Enabled = true;
            toolStripButtonStop.Enabled = false;
            timer2.Enabled = false;
            logger.Info("������������ �������� ��������� ���������� � ���������");
        }

        //����� ��� ��������� ����������� ���������� � ��������
        //����� ��� ��������� ����, ����� ���� �������� ����������, �� ��� ����� �������.
        private string GetInformation()
        {
            StringBuilder sb = new StringBuilder();

            if (processDataGridView.SelectedCells != null)
            {
                string processId;
                string processName;
                string processResponding;
                string processStartTime;
                string processHandle;
                string processPath;

                Process process = processList.Where(x => x.Id.ToString() == processDataGridView.CurrentRow.Cells[0].Value.ToString()).ToList().FirstOrDefault();
                try { processId = process.Id.ToString(); } catch(Exception) {processId = "�������� � �������"; }
                try { processName = process.ProcessName.ToString(); } catch (Exception) {processName = "�������� � �������"; }
                try { processResponding = process.Responding == true ? "���������: � ������\n" : "���������: �� ��������\n"; } catch (Exception) {processResponding = "�������� � �������\n"; }
                try { processStartTime = process.StartTime.ToString(); } catch (Exception) { processStartTime = "�������� � �������"; }
                try { processHandle = process.Handle.ToString(); } catch (Exception) { processHandle = "�������� � �������"; }
                try { processPath = process.MainModule.FileName.ToString(); } catch (Exception) { processPath = "�������� � �������"; }

                logger.Info($"������������� ��������� ���������� � �������� � ID: {process.Id}");

                sb.Append($"ID ��������: " + processId+"\n");
                sb.Append($"�������� ��������: " + processName + "\n");
                sb.Append(processResponding);
                sb.Append($"����� �������: " + processStartTime + "\n");
                sb.Append($"Handle: " + processHandle + "\n");
                sb.Append($"����: " + processPath + "\n");
            }
            return sb.ToString();
        }

        //����� ��� ��������� �������� � ���������� ������
        private void KillProcess(Process process)
        {
            int positionIndex = 0;
            int selectedPositionIndex = 0;
            if (processDataGridView.FirstDisplayedScrollingRowIndex != -1)
                positionIndex = processDataGridView.FirstDisplayedScrollingRowIndex;
            if (processDataGridView.CurrentCell != null)
                selectedPositionIndex = processDataGridView.CurrentCell.RowIndex;

            process.Kill();
            process.WaitForExit();

            if (!gettingProcesses.IsAlive)
            {
                gettingProcesses = new Thread(() =>
                {
                    lock (processToDisplay)
                    {
                        processList.Clear();
                        processList = GetProcessesWithSort();

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
        
        //����� ��� ��������� ����������� ���
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

        //����� �� ����������
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logger.Info($"���������� ���������");
            Application.Exit();
        }

        //������ ��������� ���������� �� �������
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetInformation());
        }

        //������ ��������� ��������
        private void killToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (processDataGridView.SelectedCells != null)
                {
                    Process processToKill = processList.Where(x => x.Id.ToString() == processDataGridView.CurrentRow.Cells[0].Value.ToString()).ToList().FirstOrDefault();
                    logger.Info($"������������� ��������� ������ �� ��������� �������� � ID: {processToKill.Id}");
                    KillProcess(processToKill);
                }
            }
            catch (Exception) { }
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            logger.Info($"���������� ���������");
        }

        //���������� ��������� ����������
        private void processDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var columnIndex=e.ColumnIndex;
            switch(columnIndex)
            {
                case 0:
                    GetProcessesWithSort = BasicActions.ProccesData.GetSortetProcessListById;
                    logger.Info("������������ �������� ���������� �� ID");
                    break;
                case 1:
                    GetProcessesWithSort = BasicActions.ProccesData.GetSortetProcessListByName;
                    logger.Info("������������ �������� ���������� �� �������� ��������");
                    break;
                case 2:
                    GetProcessesWithSort = BasicActions.ProccesData.GetSortetProcessListByMemory;
                    logger.Info("������������ �������� ���������� �� ���������� ��������� ������");
                    break;
                case 3:
                    GetProcessesWithSort = BasicActions.ProccesData.GetSortetProcessListByRespose;
                    logger.Info("������������ �������� ���������� �� ��������� ��������");
                    break;
            }
        }
    }
}

