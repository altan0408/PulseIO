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
            this.btnReports = new System.Windows.Forms.Button();
            this.btnLogs = new System.Windows.Forms.Button();
            this.btnTransfers = new System.Windows.Forms.Button();
            this.btnDevices = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.grpTransferActivity = new System.Windows.Forms.GroupBox();
            this.dgvTransfers = new System.Windows.Forms.DataGridView();
            this.grpDeviceTable = new System.Windows.Forms.GroupBox();
            this.dgvDevices = new System.Windows.Forms.DataGridView();
            this.pnlSeparator = new System.Windows.Forms.Panel();
            this.pnlStatistics = new System.Windows.Forms.FlowLayoutPanel();
            this.grpFailedCount = new System.Windows.Forms.GroupBox();
            this.lblFailedCount = new System.Windows.Forms.Label();
            this.grpSuccessCount = new System.Windows.Forms.GroupBox();
            this.lblSuccessCount = new System.Windows.Forms.Label();
            this.grpTransferCount = new System.Windows.Forms.GroupBox();
            this.lblTransferCount = new System.Windows.Forms.Label();
            this.grpDeviceCount = new System.Windows.Forms.GroupBox();
            this.lblDeviceCount = new System.Windows.Forms.Label();
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
            this.grpFailedCount.SuspendLayout();
            this.grpSuccessCount.SuspendLayout();
            this.grpTransferCount.SuspendLayout();
            this.grpDeviceCount.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHeader.Controls.Add(this.lblDateTime);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(10);
            this.pnlHeader.Size = new System.Drawing.Size(1200, 80);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblDateTime
            // 
            this.lblDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.Location = new System.Drawing.Point(1050, 20);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(122, 16);
            this.lblDateTime.TabIndex = 2;
            this.lblDateTime.Text = "00/00/0000 00:00:00";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSubtitle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblSubtitle.Location = new System.Drawing.Point(13, 35);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(291, 18);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "I/O Device Transfer and Monitoring System";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(10, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(118, 31);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "PulseIO";
            // 
            // pnlNavigation
            // 
            this.pnlNavigation.Controls.Add(this.btnReports);
            this.pnlNavigation.Controls.Add(this.btnLogs);
            this.pnlNavigation.Controls.Add(this.btnTransfers);
            this.pnlNavigation.Controls.Add(this.btnDevices);
            this.pnlNavigation.Controls.Add(this.btnDashboard);
            this.pnlNavigation.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlNavigation.Location = new System.Drawing.Point(0, 80);
            this.pnlNavigation.Name = "pnlNavigation";
            this.pnlNavigation.Padding = new System.Windows.Forms.Padding(10);
            this.pnlNavigation.Size = new System.Drawing.Size(200, 569);
            this.pnlNavigation.TabIndex = 1;
            // 
            // btnReports
            // 
            this.btnReports.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnReports.Location = new System.Drawing.Point(10, 170);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(130, 40);
            this.btnReports.TabIndex = 4;
            this.btnReports.Text = "Reports";
            this.btnReports.UseVisualStyleBackColor = true;
            // 
            // btnLogs
            // 
            this.btnLogs.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLogs.Location = new System.Drawing.Point(10, 130);
            this.btnLogs.Name = "btnLogs";
            this.btnLogs.Size = new System.Drawing.Size(130, 40);
            this.btnLogs.TabIndex = 3;
            this.btnLogs.Text = "Logs";
            this.btnLogs.UseVisualStyleBackColor = true;
            // 
            // btnTransfers
            // 
            this.btnTransfers.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTransfers.Location = new System.Drawing.Point(10, 90);
            this.btnTransfers.Name = "btnTransfers";
            this.btnTransfers.Size = new System.Drawing.Size(130, 40);
            this.btnTransfers.TabIndex = 2;
            this.btnTransfers.Text = "Transfers";
            this.btnTransfers.UseVisualStyleBackColor = true;
            // 
            // btnDevices
            // 
            this.btnDevices.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDevices.Location = new System.Drawing.Point(10, 50);
            this.btnDevices.Name = "btnDevices";
            this.btnDevices.Size = new System.Drawing.Size(130, 40);
            this.btnDevices.TabIndex = 1;
            this.btnDevices.Text = "Devices";
            this.btnDevices.UseVisualStyleBackColor = true;
            // 
            // btnDashboard
            // 
            this.btnDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDashboard.Location = new System.Drawing.Point(10, 10);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(130, 40);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.UseVisualStyleBackColor = true;
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.Controls.Add(this.grpTransferActivity);
            this.pnlMain.Controls.Add(this.grpDeviceTable);
            this.pnlMain.Controls.Add(this.pnlSeparator);
            this.pnlMain.Controls.Add(this.pnlStatistics);
            this.pnlMain.Controls.Add(this.lblOverview);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(150, 80);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(10);
            this.pnlMain.Size = new System.Drawing.Size(1050, 569);
            this.pnlMain.TabIndex = 2;
            // 
            // grpTransferActivity
            // 
            this.grpTransferActivity.Controls.Add(this.dgvTransfers);
            this.grpTransferActivity.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTransferActivity.Location = new System.Drawing.Point(10, 381);
            this.grpTransferActivity.Name = "grpTransferActivity";
            this.grpTransferActivity.Padding = new System.Windows.Forms.Padding(5);
            this.grpTransferActivity.Size = new System.Drawing.Size(1030, 180);
            this.grpTransferActivity.TabIndex = 3;
            this.grpTransferActivity.TabStop = false;
            this.grpTransferActivity.Text = "Recent Transfer Activity";
            // 
            // dgvTransfers
            // 
            this.dgvTransfers.AllowUserToAddRows = false;
            this.dgvTransfers.AllowUserToDeleteRows = false;
            this.dgvTransfers.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvTransfers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransfers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTransfers.Location = new System.Drawing.Point(5, 20);
            this.dgvTransfers.Name = "dgvTransfers";
            this.dgvTransfers.ReadOnly = true;
            this.dgvTransfers.RowHeadersWidth = 51;
            this.dgvTransfers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTransfers.Size = new System.Drawing.Size(1020, 155);
            this.dgvTransfers.TabIndex = 0;
            // 
            // grpDeviceTable
            // 
            this.grpDeviceTable.Controls.Add(this.dgvDevices);
            this.grpDeviceTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDeviceTable.Location = new System.Drawing.Point(10, 156);
            this.grpDeviceTable.Name = "grpDeviceTable";
            this.grpDeviceTable.Padding = new System.Windows.Forms.Padding(5);
            this.grpDeviceTable.Size = new System.Drawing.Size(1030, 225);
            this.grpDeviceTable.TabIndex = 2;
            this.grpDeviceTable.TabStop = false;
            this.grpDeviceTable.Text = "Connected Devices";
            // 
            // dgvDevices
            // 
            this.dgvDevices.AllowUserToAddRows = false;
            this.dgvDevices.AllowUserToDeleteRows = false;
            this.dgvDevices.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDevices.Location = new System.Drawing.Point(5, 20);
            this.dgvDevices.Name = "dgvDevices";
            this.dgvDevices.ReadOnly = true;
            this.dgvDevices.RowHeadersWidth = 51;
            this.dgvDevices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDevices.Size = new System.Drawing.Size(1020, 200);
            this.dgvDevices.TabIndex = 0;
            // 
            // pnlSeparator
            // 
            this.pnlSeparator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(230)))));
            this.pnlSeparator.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSeparator.Location = new System.Drawing.Point(10, 155);
            this.pnlSeparator.Name = "pnlSeparator";
            this.pnlSeparator.Size = new System.Drawing.Size(1030, 1);
            this.pnlSeparator.TabIndex = 5;
            // 
            // pnlStatistics
            // 
            this.pnlStatistics.AutoSize = true;
            this.pnlStatistics.Controls.Add(this.grpFailedCount);
            this.pnlStatistics.Controls.Add(this.grpSuccessCount);
            this.pnlStatistics.Controls.Add(this.grpTransferCount);
            this.pnlStatistics.Controls.Add(this.grpDeviceCount);
            this.pnlStatistics.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStatistics.Location = new System.Drawing.Point(10, 39);
            this.pnlStatistics.Name = "pnlStatistics";
            this.pnlStatistics.Size = new System.Drawing.Size(1030, 116);
            this.pnlStatistics.TabIndex = 1;
            // 
            // grpFailedCount
            // 
            this.grpFailedCount.Controls.Add(this.lblFailedCount);
            this.grpFailedCount.Location = new System.Drawing.Point(3, 3);
            this.grpFailedCount.Name = "grpFailedCount";
            this.grpFailedCount.Size = new System.Drawing.Size(250, 110);
            this.grpFailedCount.TabIndex = 3;
            this.grpFailedCount.TabStop = false;
            this.grpFailedCount.Text = "Failed Transfers";
            // 
            // lblFailedCount
            // 
            this.lblFailedCount.AutoSize = true;
            this.lblFailedCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold);
            this.lblFailedCount.Location = new System.Drawing.Point(100, 35);
            this.lblFailedCount.Name = "lblFailedCount";
            this.lblFailedCount.Size = new System.Drawing.Size(230, 110);
            this.lblFailedCount.TabIndex = 0;
            this.lblFailedCount.Text = "0";
            // 
            // grpSuccessCount
            // 
            this.grpSuccessCount.Controls.Add(this.lblSuccessCount);
            this.grpSuccessCount.Location = new System.Drawing.Point(259, 3);
            this.grpSuccessCount.Name = "grpSuccessCount";
            this.grpSuccessCount.Size = new System.Drawing.Size(230, 110);
            this.grpSuccessCount.TabIndex = 2;
            this.grpSuccessCount.TabStop = false;
            this.grpSuccessCount.Text = "Successful Transfers";
            // 
            // lblSuccessCount
            // 
            this.lblSuccessCount.AutoSize = true;
            this.lblSuccessCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold);
            this.lblSuccessCount.Location = new System.Drawing.Point(100, 35);
            this.lblSuccessCount.Name = "lblSuccessCount";
            this.lblSuccessCount.Size = new System.Drawing.Size(64, 69);
            this.lblSuccessCount.TabIndex = 0;
            this.lblSuccessCount.Text = "0";
            // 
            // grpTransferCount
            // 
            this.grpTransferCount.Controls.Add(this.lblTransferCount);
            this.grpTransferCount.Location = new System.Drawing.Point(515, 3);
            this.grpTransferCount.Name = "grpTransferCount";
            this.grpTransferCount.Size = new System.Drawing.Size(230, 110);
            this.grpTransferCount.TabIndex = 1;
            this.grpTransferCount.TabStop = false;
            this.grpTransferCount.Text = "Transfers Today";
            // 
            // lblTransferCount
            // 
            this.lblTransferCount.AutoSize = true;
            this.lblTransferCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold);
            this.lblTransferCount.Location = new System.Drawing.Point(100, 35);
            this.lblTransferCount.Name = "lblTransferCount";
            this.lblTransferCount.Size = new System.Drawing.Size(64, 69);
            this.lblTransferCount.TabIndex = 0;
            this.lblTransferCount.Text = "0";
            // 
            // grpDeviceCount
            // 
            this.grpDeviceCount.Controls.Add(this.lblDeviceCount);
            this.grpDeviceCount.Location = new System.Drawing.Point(771, 3);
            this.grpDeviceCount.Name = "grpDeviceCount";
            this.grpDeviceCount.Size = new System.Drawing.Size(230, 110);
            this.grpDeviceCount.TabIndex = 0;
            this.grpDeviceCount.TabStop = false;
            this.grpDeviceCount.Text = "Connected Devices";
            // 
            // lblDeviceCount
            // 
            this.lblDeviceCount.AutoSize = true;
            this.lblDeviceCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold);
            this.lblDeviceCount.Location = new System.Drawing.Point(100, 35);
            this.lblDeviceCount.Name = "lblDeviceCount";
            this.lblDeviceCount.Size = new System.Drawing.Size(64, 69);
            this.lblDeviceCount.TabIndex = 0;
            this.lblDeviceCount.Text = "0";
            // 
            // lblOverview
            // 
            this.lblOverview.AutoSize = true;
            this.lblOverview.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOverview.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblOverview.Location = new System.Drawing.Point(10, 10);
            this.lblOverview.Name = "lblOverview";
            this.lblOverview.Size = new System.Drawing.Size(214, 29);
            this.lblOverview.TabIndex = 0;
            this.lblOverview.Text = "System Overview";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSystemStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 649);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1200, 26);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblSystemStatus
            // 
            this.lblSystemStatus.Name = "lblSystemStatus";
            this.lblSystemStatus.Size = new System.Drawing.Size(50, 20);
            this.lblSystemStatus.Text = "Ready";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 675);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlNavigation);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PulseIO";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlNavigation.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpTransferActivity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransfers)).EndInit();
            this.grpDeviceTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).EndInit();
            this.pnlStatistics.ResumeLayout(false);
            this.grpFailedCount.ResumeLayout(false);
            this.grpFailedCount.PerformLayout();
            this.grpSuccessCount.ResumeLayout(false);
            this.grpSuccessCount.PerformLayout();
            this.grpTransferCount.ResumeLayout(false);
            this.grpTransferCount.PerformLayout();
            this.grpDeviceCount.ResumeLayout(false);
            this.grpDeviceCount.PerformLayout();
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
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnLogs;
        private System.Windows.Forms.Button btnTransfers;
        private System.Windows.Forms.Button btnDevices;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.GroupBox grpTransferActivity;
        private System.Windows.Forms.DataGridView dgvTransfers;
        private System.Windows.Forms.GroupBox grpDeviceTable;
        private System.Windows.Forms.DataGridView dgvDevices;
        private System.Windows.Forms.FlowLayoutPanel pnlStatistics;
        private System.Windows.Forms.GroupBox grpFailedCount;
        private System.Windows.Forms.Label lblFailedCount;
        private System.Windows.Forms.GroupBox grpSuccessCount;
        private System.Windows.Forms.Label lblSuccessCount;
        private System.Windows.Forms.GroupBox grpTransferCount;
        private System.Windows.Forms.Label lblTransferCount;
        private System.Windows.Forms.GroupBox grpDeviceCount;
        private System.Windows.Forms.Label lblDeviceCount;
        private System.Windows.Forms.Label lblOverview;
        private System.Windows.Forms.Panel pnlSeparator;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblSystemStatus;
    }
}