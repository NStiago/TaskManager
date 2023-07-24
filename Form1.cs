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
    //кастомная сортировка по кнопке
    public partial class Form1 : Form
    {
        //лист процессов
        List<Process> processList = new List<Process>();

        //отдельный поток, в котором происходит получение информации по процессам
        Thread? gettingProcesses;

        //лист процессов для отображения на главной форме
        List<ProcessForDisplaying> processToDisplay = new List<ProcessForDisplaying>();

        //вспомогательный лист для получения изменений исполняемых процессов (появление, удаление).
        //обновление списка исполняемых процессов - итеративное
        List<ProcessForDisplaying> processToDisplayBeforeUpdate = new List<ProcessForDisplaying>();

        //Логирование информации с помощью NLog
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //Делегат для кастомной сортировки (в зависимости от индекса колонки, вызывается необходимый метод)
        delegate List<Process> GetProcesses();
        GetProcesses GetProcessesWithSort = BasicActions.ProccesData.GetSortetProcessListByMemory;

        public Form1()
        {
            InitializeComponent();
        }

        //При загрузке основного формы, создается новый поток, в котором происходит инициализация исполняемых процессов
        //заполняется processDataGridView
        //пробовал реализовать через BindingSource - не достиг связи данных и обновления данных в гриде. Место для улучшения кода
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
            logger.Info("Приложение запущено");
        }

        //старт таймера для обновления данных в гриде
        private void timer2_Tick(object sender, EventArgs e)
        {
            //positionIndex и selectedPositionIndex - для запоминания выделенной строки и позиции скрола
            int positionIndex = 0;
            int selectedPositionIndex = 0;
            if (processDataGridView.FirstDisplayedScrollingRowIndex != -1)
                positionIndex = processDataGridView.FirstDisplayedScrollingRowIndex;
            if (processDataGridView.CurrentCell != null)
                selectedPositionIndex = processDataGridView.CurrentCell.RowIndex;

            //повторный запуск процесса по таймеру
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

                        //получение изменных исполняемых процессов. Место для улучшени кода, поскольку не совсем эффективная реализация
                        foreach (var process in processToDisplay)
                            if (!processToDisplayBeforeUpdate.Any(x => x.ProcessId == process.ProcessId))
                                logger.Info($"В системе запущен процесс {process.ProcessName} c ID: {process.ProcessId}");

                        foreach (var process in processToDisplayBeforeUpdate)
                            if (!processToDisplay.Any(x => x.ProcessId == process.ProcessId))
                                logger.Info($"В системе остановлен процесс {process.ProcessName} c ID: {process.ProcessId}");
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

        //кнопка для продолжения потока получения информации по процессам
        private void toolStripButtonStart_Click(object sender, EventArgs e)
        {
            toolStripButtonStop.Enabled = true;
            toolStripButtonStart.Enabled = false;
            timer2.Enabled = true;
            logger.Info("Возобновление процесса получения информации о процессах");
        }

        //кнопка для приостановки потока получения информации по процессам
        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            toolStripButtonStart.Enabled = true;
            toolStripButtonStop.Enabled = false;
            timer2.Enabled = false;
            logger.Info("Приостановка процесса получения информации о процессах");
        }

        //метод для получения расширенной информации о процессе
        //Место для улучшения кода, можно было написать лаконичнее, но так более читаемо.
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
                try { processId = process.Id.ToString(); } catch(Exception) {processId = "Отказано в доступе"; }
                try { processName = process.ProcessName.ToString(); } catch (Exception) {processName = "Отказано в доступе"; }
                try { processResponding = process.Responding == true ? "Состояние: в работе\n" : "Состояние: не отвечает\n"; } catch (Exception) {processResponding = "Отказано в доступе\n"; }
                try { processStartTime = process.StartTime.ToString(); } catch (Exception) { processStartTime = "Отказано в доступе"; }
                try { processHandle = process.Handle.ToString(); } catch (Exception) { processHandle = "Отказано в доступе"; }
                try { processPath = process.MainModule.FileName.ToString(); } catch (Exception) { processPath = "Отказано в доступе"; }

                logger.Info($"Пользователем запрошена информация о процессе с ID: {process.Id}");

                sb.Append($"ID процесса: " + processId+"\n");
                sb.Append($"Название процесса: " + processName + "\n");
                sb.Append(processResponding);
                sb.Append($"Время запуска: " + processStartTime + "\n");
                sb.Append($"Handle: " + processHandle + "\n");
                sb.Append($"Путь: " + processPath + "\n");
            }
            return sb.ToString();
        }

        //метод для остановки процесса и обновления списка
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
        
        //метод для изменения функционала ПКМ
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

        //выход из приложения
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logger.Info($"Приложение выключено");
            Application.Exit();
        }

        //кнопка получения информации по потокам
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetInformation());
        }

        //кнопка остановки процесса
        private void killToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (processDataGridView.SelectedCells != null)
                {
                    Process processToKill = processList.Where(x => x.Id.ToString() == processDataGridView.CurrentRow.Cells[0].Value.ToString()).ToList().FirstOrDefault();
                    logger.Info($"Пользователем отправлен запрос на остановку процесса с ID: {processToKill.Id}");
                    KillProcess(processToKill);
                }
            }
            catch (Exception) { }
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            logger.Info($"Приложение выключено");
        }

        //реализация кастомной сортировки
        private void processDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var columnIndex=e.ColumnIndex;
            switch(columnIndex)
            {
                case 0:
                    GetProcessesWithSort = BasicActions.ProccesData.GetSortetProcessListById;
                    logger.Info("Пользователь произвел сортировку по ID");
                    break;
                case 1:
                    GetProcessesWithSort = BasicActions.ProccesData.GetSortetProcessListByName;
                    logger.Info("Пользователь произвел сортировку по названию процесса");
                    break;
                case 2:
                    GetProcessesWithSort = BasicActions.ProccesData.GetSortetProcessListByMemory;
                    logger.Info("Пользователь произвел сортировку по занимаемой процессом памяти");
                    break;
                case 3:
                    GetProcessesWithSort = BasicActions.ProccesData.GetSortetProcessListByRespose;
                    logger.Info("Пользователь произвел сортировку по состоянию процесса");
                    break;
            }
        }
    }
}

