using System;
using System.Drawing;
using System.Windows.Forms;

namespace PulseIO
{
    public class DeviceDetailsForm : Form
    {
        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel pnlContent;
        private Button btnClose;

        public DeviceDetailsForm(string deviceName, string deviceType, string status, DateTime connectedAt, string pnpId, string manufacturer)
        {
            InitializeComponent(deviceName, deviceType, status, connectedAt, pnpId, manufacturer);
        }

        private void InitializeComponent(string name, string type, string status, DateTime connectedAt, string pnpId, string manufacturer)
        {
            // Form setup
            this.Size = new Size(550, 480);
            this.Text = "Device Details - PulseIO";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(241, 245, 249);
            this.Font = new Font("Segoe UI", 9.75F);

            // Header Panel
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 75,
                BackColor = Color.White,
                Padding = new Padding(20, 15, 20, 15)
            };
            pnlHeader.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(226, 232, 240)))
                    e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            };

            lblTitle = new Label
            {
                Text = "Device Diagnostics",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(47, 72, 88),
                AutoSize = true,
                Location = new Point(20, 12)
            };

            lblSubtitle = new Label
            {
                Text = "Detailed hardware metadata from WMI provider",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(78, 88, 123),
                AutoSize = true,
                Location = new Point(22, 42)
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // Content Panel
            pnlContent = new Panel
            {
                Location = new Point(20, 95),
                Size = new Size(495, 275),
                BackColor = Color.White,
                Padding = new Padding(20)
            };
            pnlContent.Paint += (s, e) =>
            {
                var rect = new Rectangle(0, 0, pnlContent.Width - 1, pnlContent.Height - 1);
                using (var pen = new Pen(Color.FromArgb(226, 232, 240)))
                    e.Graphics.DrawRectangle(pen, rect);
            };

            int startY = 20;
            int rowHeight = 38;

            AddDetailRow(pnlContent, "Device Name", name, ref startY, rowHeight);
            AddDetailRow(pnlContent, "Device Type", type, ref startY, rowHeight);
            AddDetailRow(pnlContent, "Status", status, ref startY, rowHeight);

            TimeSpan uptime = DateTime.Now - connectedAt;
            string uptimeStr = uptime.TotalMinutes < 1
                ? $"{(int)uptime.TotalSeconds}s"
                : uptime.TotalHours < 1
                    ? $"{(int)uptime.TotalMinutes}m {uptime.Seconds:D2}s"
                    : $"{(int)uptime.TotalHours}h {uptime.Minutes:D2}m";
            string timeVal = $"{connectedAt:MM/dd/yyyy HH:mm:ss} (Active for {uptimeStr})";
            AddDetailRow(pnlContent, "Connected Time", timeVal, ref startY, rowHeight);

            AddDetailRow(pnlContent, "Manufacturer", manufacturer, ref startY, rowHeight);
            AddDetailRow(pnlContent, "PNP Device ID", pnpId, ref startY, rowHeight, isLongText: true);

            // Close Button
            btnClose = new Button
            {
                Text = "Close",
                Size = new Size(100, 38),
                Location = new Point(415, 385),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(47, 72, 88),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(78, 88, 123);
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlContent);
            this.Controls.Add(btnClose);
            this.AcceptButton = btnClose;
        }

        private void AddDetailRow(Panel parent, string labelText, string valueText, ref int y, int height, bool isLongText = false)
        {
            var lblName = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(78, 88, 123),
                Location = new Point(15, y),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblValue = new Label
            {
                Text = valueText,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(47, 72, 88),
                Location = new Point(140, y),
                Size = new Size(335, isLongText ? 40 : 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            parent.Controls.Add(lblName);
            parent.Controls.Add(lblValue);

            y += height + (isLongText ? 15 : 0);
        }
    }
}