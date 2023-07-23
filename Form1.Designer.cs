namespace TaskManager__Businescope_
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            menuStrip1 = new MenuStrip();
            файлToolStripMenuItem = new ToolStripMenuItem();
            выходToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1 = new ToolStrip();
            toolStripButtonStart = new ToolStripButton();
            toolStripButtonStop = new ToolStripButton();
            contextMenuStrip1 = new ContextMenuStrip(components);
            получитьИнформациюToolStripMenuItem = new ToolStripMenuItem();
            завершитьToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            labelCount = new Label();
            timer2 = new System.Windows.Forms.Timer(components);
            processGridView = new DataGridView();
            ProcessId = new DataGridViewTextBoxColumn();
            ProcessName = new DataGridViewTextBoxColumn();
            ProcessMemory = new DataGridViewTextBoxColumn();
            IsResponding = new DataGridViewTextBoxColumn();
            menuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)processGridView).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { файлToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(780, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            файлToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { выходToolStripMenuItem });
            файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            файлToolStripMenuItem.Size = new Size(48, 20);
            файлToolStripMenuItem.Text = "Файл";
            // 
            // выходToolStripMenuItem
            // 
            выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            выходToolStripMenuItem.Size = new Size(109, 22);
            выходToolStripMenuItem.Text = "Выход";
            выходToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButtonStart, toolStripButtonStop });
            toolStrip1.Location = new Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(780, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonStart
            // 
            toolStripButtonStart.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonStart.Image = (Image)resources.GetObject("toolStripButtonStart.Image");
            toolStripButtonStart.ImageTransparentColor = Color.Magenta;
            toolStripButtonStart.Name = "toolStripButtonStart";
            toolStripButtonStart.Size = new Size(81, 22);
            toolStripButtonStart.Text = "Продолжить";
            toolStripButtonStart.Click += toolStripButtonStart_Click;
            // 
            // toolStripButtonStop
            // 
            toolStripButtonStop.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonStop.Image = (Image)resources.GetObject("toolStripButtonStop.Image");
            toolStripButtonStop.ImageTransparentColor = Color.Magenta;
            toolStripButtonStop.Name = "toolStripButtonStop";
            toolStripButtonStop.Size = new Size(96, 22);
            toolStripButtonStop.Text = "Приостановить";
            toolStripButtonStop.Click += toolStripButtonStop_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { получитьИнформациюToolStripMenuItem, завершитьToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(208, 48);
            // 
            // получитьИнформациюToolStripMenuItem
            // 
            получитьИнформациюToolStripMenuItem.Name = "получитьИнформациюToolStripMenuItem";
            получитьИнформациюToolStripMenuItem.Size = new Size(207, 22);
            получитьИнформациюToolStripMenuItem.Text = "Получить информацию";
            получитьИнформациюToolStripMenuItem.Click += получитьИнформациюToolStripMenuItem_Click;
            // 
            // завершитьToolStripMenuItem
            // 
            завершитьToolStripMenuItem.Name = "завершитьToolStripMenuItem";
            завершитьToolStripMenuItem.Size = new Size(207, 22);
            завершитьToolStripMenuItem.Text = "Завершить";
            завершитьToolStripMenuItem.Click += CloseToolStripMenuItem_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.Location = new Point(12, 392);
            label1.Name = "label1";
            label1.Size = new Size(103, 15);
            label1.TabIndex = 3;
            label1.Text = "Всего процессов:";
            // 
            // labelCount
            // 
            labelCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelCount.AutoSize = true;
            labelCount.Location = new Point(121, 392);
            labelCount.Name = "labelCount";
            labelCount.Size = new Size(0, 15);
            labelCount.TabIndex = 4;
            // 
            // timer2
            // 
            timer2.Enabled = true;
            timer2.Interval = 200;
            timer2.Tick += timer2_Tick;
            // 
            // processGridView
            // 
            processGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            processGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            processGridView.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = Color.DarkBlue;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            processGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            processGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            processGridView.Columns.AddRange(new DataGridViewColumn[] { ProcessId, ProcessName, ProcessMemory, IsResponding });
            processGridView.ContextMenuStrip = contextMenuStrip1;
            processGridView.EnableHeadersVisualStyles = false;
            processGridView.Location = new Point(0, 52);
            processGridView.Name = "processGridView";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.MenuHighlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            processGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            processGridView.ScrollBars = ScrollBars.Vertical;
            processGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            processGridView.Size = new Size(780, 337);
            processGridView.TabIndex = 5;
            processGridView.MouseDown += processGridView_MouseDown;
            // 
            // ProcessId
            // 
            ProcessId.DataPropertyName = "ProcessId";
            ProcessId.HeaderText = "ID процесса";
            ProcessId.Name = "ProcessId";
            ProcessId.ReadOnly = true;
            // 
            // ProcessName
            // 
            ProcessName.DataPropertyName = "ProcessName";
            ProcessName.HeaderText = "Название процесса";
            ProcessName.Name = "ProcessName";
            ProcessName.ReadOnly = true;
            // 
            // ProcessMemory
            // 
            ProcessMemory.DataPropertyName = "ProcessMemory";
            ProcessMemory.HeaderText = "Обьем памяти, МБ";
            ProcessMemory.Name = "ProcessMemory";
            ProcessMemory.ReadOnly = true;
            // 
            // IsResponding
            // 
            IsResponding.DataPropertyName = "IsResponding";
            IsResponding.HeaderText = "Состояние";
            IsResponding.Name = "IsResponding";
            IsResponding.ReadOnly = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(780, 416);
            Controls.Add(processGridView);
            Controls.Add(labelCount);
            Controls.Add(label1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Task Manager (Businescope)";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)processGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem файлToolStripMenuItem;
        private ToolStripMenuItem выходToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButtonStart;
        private ToolStripButton toolStripButtonStop;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem получитьИнформациюToolStripMenuItem;
        private ToolStripMenuItem завершитьToolStripMenuItem;
        private Label label1;
        private Label labelCount;
        private System.Windows.Forms.Timer timer2;
        private ToolStripMenuItem завершитьДеревоПроцессовToolStripMenuItem;
        private DataGridView processGridView;
        private DataGridViewTextBoxColumn ProcessId;
        private DataGridViewTextBoxColumn ProcessName;
        private DataGridViewTextBoxColumn ProcessMemory;
        private DataGridViewTextBoxColumn IsResponding;
    }
}