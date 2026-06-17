namespace PulseIO
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlNavigation = new System.Windows.Forms.Panel();
            this.btnHistory = new PulseIO.ModernButton();
            this.btnReports = new PulseIO.ModernButton();
            this.btnLogs = new PulseIO.ModernButton();
            this.btnTransfers = new PulseIO.ModernButton();
            this.btnDevices = new PulseIO.ModernButton();
            this.btnDashboard = new PulseIO.ModernButton();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.grpTransferActivity = new PulseIO.ModernCard();
            this.dgvTransfers = new System.Windows.Forms.DataGridView();
            this.grpDeviceTable = new PulseIO.ModernCard();
            this.dgvDevices = new System.Windows.Forms.DataGridView();
            this.pnlSeparator = new System.Windows.Forms.Panel();
            this.pnlStatistics = new System.Windows.Forms.FlowLayoutPanel();
            this.grpDeviceCount = new PulseIO.ModernCard();
            this.lblDeviceCount = new System.Windows.Forms.Label();
            this.grpTransferCount = new PulseIO.ModernCard();
            this.lblTransferCount = new System.Windows.Forms.Label();
            this.grpSuccessCount = new PulseIO.ModernCard();
            this.lblSuccessCount = new System.Windows.Forms.Label();
            this.grpFailedCount = new PulseIO.ModernCard();
            this.lblFailedCount = new System.Windows.Forms.Label();
            this.lblOverview = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblSystemStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlHeader.SuspendLayout();
            this.pnlNavigation.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.grpTransferActivity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransfers)).BeginInit();
            this.grpDeviceTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).BeginInit();
            this.pnlStatistics.SuspendLayout();
            this.grpDeviceCount.SuspendLayout();
            this.grpTransferCount.SuspendLayout();
            this.grpSuccessCount.SuspendLayout();
            this.grpFailedCount.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.lblDateTime);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(24, 16, 24, 16);
            this.pnlHeader.Size = new System.Drawing.Size(1200, 72);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblDateTime
            // 
            this.lblDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDateTime.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblDateTime.Location = new System.Drawing.Point(916, 22);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(260, 28);
            this.lblDateTime.TabIndex = 2;
            this.lblDateTime.Text = "00/00/0000 00:00:00";
            this.lblDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSubtitle.Location = new System.Drawing.Point(27, 40);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(291, 19);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "I/O Device Transfer and Monitoring System";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTitle.Location = new System.Drawing.Point(24, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(99, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "PulseIO";
            // 
            // pnlNavigation
            // 
            this.pnlNavigation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlNavigation.Controls.Add(this.btnHistory);
            this.pnlNavigation.Controls.Add(this.btnReports);
            this.pnlNavigation.Controls.Add(this.btnLogs);
            this.pnlNavigation.Controls.Add(this.btnTransfers);
            this.pnlNavigation.Controls.Add(this.btnDevices);
            this.pnlNavigation.Controls.Add(this.btnDashboard);
            this.pnlNavigation.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlNavigation.Location = new System.Drawing.Point(0, 72);
            this.pnlNavigation.Name = "pnlNavigation";
            this.pnlNavigation.Padding = new System.Windows.Forms.Padding(12, 20, 12, 12);
            this.pnlNavigation.Size = new System.Drawing.Size(220, 577);
            this.pnlNavigation.TabIndex = 1;
            // 
            // btnHistory
            // 
            this.btnHistory.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHistory.FlatAppearance.BorderSize = 0;
            this.btnHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnHistory.Location = new System.Drawing.Point(12, 250);
            this.btnHistory.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnHistory.Size = new System.Drawing.Size(196, 46);
            this.btnHistory.TabIndex = 5;
            this.btnHistory.Text = "History";
            this.btnHistory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHistory.UseVisualStyleBackColor = true;
            // 
            // btnReports
            // 
            this.btnReports.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnReports.FlatAppearance.BorderSize = 0;
            this.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReports.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnReports.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnReports.Location = new System.Drawing.Point(12, 204);
            this.btnReports.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnReports.Name = "btnReports";
            this.btnReports.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnReports.Size = new System.Drawing.Size(196, 46);
            this.btnReports.TabIndex = 4;
            this.btnReports.Text = "Reports";
            this.btnReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReports.UseVisualStyleBackColor = true;
            // 
            // btnLogs
            // 
            this.btnLogs.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLogs.FlatAppearance.BorderSize = 0;
            this.btnLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogs.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnLogs.Location = new System.Drawing.Point(12, 158);
            this.btnLogs.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnLogs.Name = "btnLogs";
            this.btnLogs.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnLogs.Size = new System.Drawing.Size(196, 46);
            this.btnLogs.TabIndex = 3;
            this.btnLogs.Text = "Logs";
            this.btnLogs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogs.UseVisualStyleBackColor = true;
            // 
            // btnTransfers
            // 
            this.btnTransfers.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTransfers.FlatAppearance.BorderSize = 0;
            this.btnTransfers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransfers.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnTransfers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnTransfers.Location = new System.Drawing.Point(12, 112);
            this.btnTransfers.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnTransfers.Name = "btnTransfers";
            this.btnTransfers.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnTransfers.Size = new System.Drawing.Size(196, 46);
            this.btnTransfers.TabIndex = 2;
            this.btnTransfers.Text = "Transfers";
            this.btnTransfers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTransfers.UseVisualStyleBackColor = true;
            // 
            // btnDevices
            // 
            this.btnDevices.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDevices.FlatAppearance.BorderSize = 0;
            this.btnDevices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDevices.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnDevices.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnDevices.Location = new System.Drawing.Point(12, 66);
            this.btnDevices.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnDevices.Name = "btnDevices";
            this.btnDevices.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnDevices.Size = new System.Drawing.Size(196, 46);
            this.btnDevices.TabIndex = 1;
            this.btnDevices.Text = "Devices";
            this.btnDevices.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDevices.UseVisualStyleBackColor = true;
            // 
            // btnDashboard
            // 
            this.btnDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnDashboard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnDashboard.Location = new System.Drawing.Point(12, 20);
            this.btnDashboard.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnDashboard.Size = new System.Drawing.Size(196, 46);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.UseVisualStyleBackColor = true;
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.pnlMain.Controls.Add(this.grpTransferActivity);
            this.pnlMain.Controls.Add(this.grpDeviceTable);
            this.pnlMain.Controls.Add(this.pnlSeparator);
            this.pnlMain.Controls.Add(this.pnlStatistics);
            this.pnlMain.Controls.Add(this.lblOverview);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(220, 72);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(24, 20, 24, 24);
            this.pnlMain.Size = new System.Drawing.Size(980, 577);
            this.pnlMain.TabIndex = 2;
            // 
            // grpTransferActivity
            // 
            this.grpTransferActivity.Controls.Add(this.dgvTransfers);
            this.grpTransferActivity.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTransferActivity.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.grpTransferActivity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.grpTransferActivity.Location = new System.Drawing.Point(24, 459);
            this.grpTransferActivity.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.grpTransferActivity.Name = "grpTransferActivity";
            this.grpTransferActivity.Padding = new System.Windows.Forms.Padding(16, 8, 16, 16);
            this.grpTransferActivity.Size = new System.Drawing.Size(932, 220);
            this.grpTransferActivity.TabIndex = 3;
            this.grpTransferActivity.TabStop = false;
            this.grpTransferActivity.Text = "Recent Transfer Activity";
            // 
            // dgvTransfers
            // 
            this.dgvTransfers.AllowUserToAddRows = false;
            this.dgvTransfers.AllowUserToDeleteRows = false;
            this.dgvTransfers.BackgroundColor = System.Drawing.Color.White;
            this.dgvTransfers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTransfers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransfers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTransfers.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.dgvTransfers.Location = new System.Drawing.Point(16, 27);
            this.dgvTransfers.Name = "dgvTransfers";
            this.dgvTransfers.ReadOnly = true;
            this.dgvTransfers.RowHeadersVisible = false;
            this.dgvTransfers.RowHeadersWidth = 51;
            this.dgvTransfers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTransfers.Size = new System.Drawing.Size(900, 177);
            this.dgvTransfers.TabIndex = 0;
            // 
            // grpDeviceTable
            // 
            this.grpDeviceTable.Controls.Add(this.dgvDevices);
            this.grpDeviceTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDeviceTable.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.grpDeviceTable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.grpDeviceTable.Location = new System.Drawing.Point(24, 207);
            this.grpDeviceTable.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.grpDeviceTable.Name = "grpDeviceTable";
            this.grpDeviceTable.Padding = new System.Windows.Forms.Padding(16, 8, 16, 16);
            this.grpDeviceTable.Size = new System.Drawing.Size(932, 252);
            this.grpDeviceTable.TabIndex = 2;
            this.grpDeviceTable.TabStop = false;
            this.grpDeviceTable.Text = "Connected Devices";
            // 
            // dgvDevices
            // 
            this.dgvDevices.AllowUserToAddRows = false;
            this.dgvDevices.AllowUserToDeleteRows = false;
            this.dgvDevices.BackgroundColor = System.Drawing.Color.White;
            this.dgvDevices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDevices.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.dgvDevices.Location = new System.Drawing.Point(16, 27);
            this.dgvDevices.Name = "dgvDevices";
            this.dgvDevices.ReadOnly = true;
            this.dgvDevices.RowHeadersVisible = false;
            this.dgvDevices.RowHeadersWidth = 51;
            this.dgvDevices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDevices.Size = new System.Drawing.Size(900, 209);
            this.dgvDevices.TabIndex = 0;
            // 
            // pnlSeparator
            // 
            this.pnlSeparator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlSeparator.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSeparator.Location = new System.Drawing.Point(24, 196);
            this.pnlSeparator.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.pnlSeparator.Name = "pnlSeparator";
            this.pnlSeparator.Size = new System.Drawing.Size(932, 2);
            this.pnlSeparator.TabIndex = 5;
            // 
            // pnlStatistics
            // 
            this.pnlStatistics.Controls.Add(this.grpDeviceCount);
            this.pnlStatistics.Controls.Add(this.grpTransferCount);
            this.pnlStatistics.Controls.Add(this.grpSuccessCount);
            this.pnlStatistics.Controls.Add(this.grpFailedCount);
            this.pnlStatistics.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStatistics.Location = new System.Drawing.Point(24, 56);
            this.pnlStatistics.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.pnlStatistics.Name = "pnlStatistics";
            this.pnlStatistics.Padding = new System.Windows.Forms.Padding(0, 4, 0, 8);
            this.pnlStatistics.Size = new System.Drawing.Size(932, 140);
            this.pnlStatistics.TabIndex = 1;
            this.pnlStatistics.WrapContents = false;
            // 
            // grpDeviceCount
            // 
            this.grpDeviceCount.Controls.Add(this.lblDeviceCount);
            this.grpDeviceCount.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpDeviceCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.grpDeviceCount.Location = new System.Drawing.Point(0, 8);
            this.grpDeviceCount.Margin = new System.Windows.Forms.Padding(0, 4, 16, 4);
            this.grpDeviceCount.Name = "grpDeviceCount";
            this.grpDeviceCount.Padding = new System.Windows.Forms.Padding(16, 12, 16, 16);
            this.grpDeviceCount.Size = new System.Drawing.Size(221, 120);
            this.grpDeviceCount.TabIndex = 0;
            this.grpDeviceCount.TabStop = false;
            this.grpDeviceCount.Text = "Connected Devices";
            // 
            // lblDeviceCount
            // 
            this.lblDeviceCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDeviceCount.Font = new System.Drawing.Font("Segoe UI Semibold", 32F, System.Drawing.FontStyle.Bold);
            this.lblDeviceCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblDeviceCount.Location = new System.Drawing.Point(16, 28);
            this.lblDeviceCount.Name = "lblDeviceCount";
            this.lblDeviceCount.Size = new System.Drawing.Size(189, 76);
            this.lblDeviceCount.TabIndex = 0;
            this.lblDeviceCount.Text = "0";
            this.lblDeviceCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpTransferCount
            // 
            this.grpTransferCount.Controls.Add(this.lblTransferCount);
            this.grpTransferCount.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpTransferCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.grpTransferCount.Location = new System.Drawing.Point(237, 8);
            this.grpTransferCount.Margin = new System.Windows.Forms.Padding(0, 4, 16, 4);
            this.grpTransferCount.Name = "grpTransferCount";
            this.grpTransferCount.Padding = new System.Windows.Forms.Padding(16, 12, 16, 16);
            this.grpTransferCount.Size = new System.Drawing.Size(221, 120);
            this.grpTransferCount.TabIndex = 1;
            this.grpTransferCount.TabStop = false;
            this.grpTransferCount.Text = "Transfers Today";
            // 
            // lblTransferCount
            // 
            this.lblTransferCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTransferCount.Font = new System.Drawing.Font("Segoe UI Semibold", 32F, System.Drawing.FontStyle.Bold);
            this.lblTransferCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(58)))), ((int)(((byte)(237)))));
            this.lblTransferCount.Location = new System.Drawing.Point(16, 28);
            this.lblTransferCount.Name = "lblTransferCount";
            this.lblTransferCount.Size = new System.Drawing.Size(189, 76);
            this.lblTransferCount.TabIndex = 0;
            this.lblTransferCount.Text = "0";
            this.lblTransferCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpSuccessCount
            // 
            this.grpSuccessCount.Controls.Add(this.lblSuccessCount);
            this.grpSuccessCount.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpSuccessCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.grpSuccessCount.Location = new System.Drawing.Point(474, 8);
            this.grpSuccessCount.Margin = new System.Windows.Forms.Padding(0, 4, 16, 4);
            this.grpSuccessCount.Name = "grpSuccessCount";
            this.grpSuccessCount.Padding = new System.Windows.Forms.Padding(16, 12, 16, 16);
            this.grpSuccessCount.Size = new System.Drawing.Size(221, 120);
            this.grpSuccessCount.TabIndex = 2;
            this.grpSuccessCount.TabStop = false;
            this.grpSuccessCount.Text = "Successful Transfers";
            // 
            // lblSuccessCount
            // 
            this.lblSuccessCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSuccessCount.Font = new System.Drawing.Font("Segoe UI Semibold", 32F, System.Drawing.FontStyle.Bold);
            this.lblSuccessCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblSuccessCount.Location = new System.Drawing.Point(16, 28);
            this.lblSuccessCount.Name = "lblSuccessCount";
            this.lblSuccessCount.Size = new System.Drawing.Size(189, 76);
            this.lblSuccessCount.TabIndex = 0;
            this.lblSuccessCount.Text = "0";
            this.lblSuccessCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpFailedCount
            // 
            this.grpFailedCount.Controls.Add(this.lblFailedCount);
            this.grpFailedCount.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpFailedCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.grpFailedCount.Location = new System.Drawing.Point(711, 8);
            this.grpFailedCount.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.grpFailedCount.Name = "grpFailedCount";
            this.grpFailedCount.Padding = new System.Windows.Forms.Padding(16, 12, 16, 16);
            this.grpFailedCount.Size = new System.Drawing.Size(221, 120);
            this.grpFailedCount.TabIndex = 3;
            this.grpFailedCount.TabStop = false;
            this.grpFailedCount.Text = "Failed Transfers";
            // 
            // lblFailedCount
            // 
            this.lblFailedCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFailedCount.Font = new System.Drawing.Font("Segoe UI Semibold", 32F, System.Drawing.FontStyle.Bold);
            this.lblFailedCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.lblFailedCount.Location = new System.Drawing.Point(16, 28);
            this.lblFailedCount.Name = "lblFailedCount";
            this.lblFailedCount.Size = new System.Drawing.Size(189, 76);
            this.lblFailedCount.TabIndex = 0;
            this.lblFailedCount.Text = "0";
            this.lblFailedCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOverview
            // 
            this.lblOverview.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOverview.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblOverview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblOverview.Location = new System.Drawing.Point(24, 20);
            this.lblOverview.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblOverview.Name = "lblOverview";
            this.lblOverview.Size = new System.Drawing.Size(932, 36);
            this.lblOverview.TabIndex = 0;
            this.lblOverview.Text = "System Overview";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.White;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSystemStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 649);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(12, 0, 8, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1200, 26);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblSystemStatus
            // 
            this.lblSystemStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblSystemStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.lblSystemStatus.Name = "lblSystemStatus";
            this.lblSystemStatus.Size = new System.Drawing.Size(47, 21);
            this.lblSystemStatus.Text = "Ready";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1200, 675);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlNavigation);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PulseIO";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlNavigation.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.grpTransferActivity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransfers)).EndInit();
            this.grpDeviceTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).EndInit();
            this.pnlStatistics.ResumeLayout(false);
            this.grpDeviceCount.ResumeLayout(false);
            this.grpTransferCount.ResumeLayout(false);
            this.grpSuccessCount.ResumeLayout(false);
            this.grpFailedCount.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlNavigation;
        private PulseIO.ModernButton btnHistory;
        private PulseIO.ModernButton btnReports;
        private PulseIO.ModernButton btnLogs;
        private PulseIO.ModernButton btnTransfers;
        private PulseIO.ModernButton btnDevices;
        private PulseIO.ModernButton btnDashboard;
        private System.Windows.Forms.Panel pnlMain;
        private PulseIO.ModernCard grpTransferActivity;
        private System.Windows.Forms.DataGridView dgvTransfers;
        private PulseIO.ModernCard grpDeviceTable;
        private System.Windows.Forms.DataGridView dgvDevices;
        private System.Windows.Forms.FlowLayoutPanel pnlStatistics;
        private PulseIO.ModernCard grpFailedCount;
        private System.Windows.Forms.Label lblFailedCount;
        private PulseIO.ModernCard grpSuccessCount;
        private System.Windows.Forms.Label lblSuccessCount;
        private PulseIO.ModernCard grpTransferCount;
        private System.Windows.Forms.Label lblTransferCount;
        private PulseIO.ModernCard grpDeviceCount;
        private System.Windows.Forms.Label lblDeviceCount;
        private System.Windows.Forms.Label lblOverview;
        private System.Windows.Forms.Panel pnlSeparator;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblSystemStatus;
    }
}
