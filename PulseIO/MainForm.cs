using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PulseIO
{
    public partial class MainForm : Form
    {
        // ── Controls created in code ──────────────────────────────────────
        private TextBox txtLogs;
        private TextBox txtReports;
        private TextBox txtHistory;
        private ModernButton btnSaveReport;
        private ModernButton btnClearHistory;
        private ModernButton btnExportHistory;

        // ── View Containers and Wrapper Panels ────────────────────────────
        private Panel pnlLogsContainer;
        private Panel pnlReportsContainer;
        private Panel pnlHistoryContainer;
        private RoundedPanel pnlLogsWrapper;
        private RoundedPanel pnlReportsWrapper;
        private RoundedPanel pnlHistoryWrapper;
        private Panel pnlReportsBottom;
        private Panel pnlHistoryBottom;

        // ── CPU & RAM Usage Cards ─────────────────────────────────────────
        private ModernCard grpCpuUsage;
        private Label lblCpuUsage;
        private ModernCard grpRamUsage;
        private Label lblRamUsage;

        // ── Dashboard System Health Section ───────────────────────────────
        private ModernCard grpSystemHealth;
        private Label lblHealthCpuVal;
        private Label lblHealthRamVal;
        private Label lblHealthDevicesVal;
        private Label lblHealthTransfersVal;

        // ── Device Search Controls ────────────────────────────────────────
        private TextBox txtSearchDevices;
        private Label lblSearchDevices;

        // ── System Performance Monitoring ─────────────────────────────────
        private PerformanceCounter cpuCounter;

        // ── Status bar buffer labels ──────────────────────────────────────
        private System.Windows.Forms.ToolStripStatusLabel tsslBufferSize;
        private System.Windows.Forms.ToolStripStatusLabel tsslFlushCount;

        // ── Timers ────────────────────────────────────────────────────────
        private Timer statsTimer;
        private Timer diskIoTimer;
        private Timer uptimeTimer;
        private Timer dateTimeTimer;
        private Scheduler scheduler = new Scheduler();

        // ── Device state — keyed by PNPDeviceID ──────────────────────────
        private Dictionary<string, (string Name, DateTime ConnectedAt, uint ErrCode)>
            deviceCache = new Dictionary<string, (string, DateTime, uint)>();

        // ── Session stats for Reports tab ─────────────────────────────────
        private int _sessionConnects = 0;
        private int _sessionDisconnects = 0;
        private double _peakReadMBs = 0;
        private double _peakWriteMBs = 0;
        private DateTime _sessionStart = DateTime.Now;

        // ── Disk I/O state ────────────────────────────────────────────────
        private Dictionary<string, ulong> prevReadBytes = new Dictionary<string, ulong>();
        private Dictionary<string, ulong> prevWriteBytes = new Dictionary<string, ulong>();

        // ── WM_DEVICECHANGE constants ─────────────────────────────────────
        private const int WM_DEVICECHANGE = 0x0219;
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        private const int DBT_DEVNODES_CHANGED = 0x0007;

        private const int MAX_TRANSFER_ROWS = 300;

        // ── I/O Buffer ────────────────────────────────────────────────────
        private BufferManager _bufferManager;
        private Timer _bufferTimer;

        // ── Device Manager ────────────────────────────────────────────────
        private DeviceManager _deviceManager;

        // History manager
        private HistoryManager _historyManager;

        // ── Debounce for DBT_DEVNODES_CHANGED ────────────────────────────
        private System.Threading.Timer _hidDebounceTimer;
        private readonly object _hidLock = new object();

        // ── Theme palette (UI only) ─────────────────────────────────────
        private static readonly Color SidebarBg = Color.FromArgb(15, 23, 42);
        private static readonly Color SidebarHover = Color.FromArgb(30, 41, 59);
        private static readonly Color SidebarActive = Color.FromArgb(51, 65, 85);
        private static readonly Color ContentBg = Color.FromArgb(241, 245, 249);
        private static readonly Color CardBg = Color.White;
        private static readonly Color TextPrimary = Color.FromArgb(30, 41, 59);
        private static readonly Color TextMuted = Color.FromArgb(100, 116, 139);
        private static readonly Color BorderSubtle = Color.FromArgb(226, 232, 240);
        private static readonly Color GridHeaderBg = Color.FromArgb(30, 41, 59);
        private static readonly Color GridAltRow = Color.FromArgb(248, 250, 252);
        private static readonly Color GridSelectionBg = Color.FromArgb(219, 234, 254);
        private static readonly Color GridSelectionFg = Color.FromArgb(30, 41, 59);
        private static readonly Color AccentBlue = Color.FromArgb(37, 99, 235);
        private static readonly Color AccentViolet = Color.FromArgb(124, 58, 237);
        private static readonly Color AccentGreen = Color.FromArgb(22, 163, 74);
        private static readonly Color AccentAmber = Color.FromArgb(217, 119, 6);

        private Button _activeNavButton;

        // =================================================================
        public MainForm()
        {
            InitializeComponent();
            ApplyModernTheme();
            InitializeDataGridViewColumns();
            CreateLogsAndReportsControls();
            CreateStatusBarControls();

            InitializePerformanceCounters();
            CreateCpuRamCards();
            CreateSearchControls();
            CreateSystemHealthSection();
            CreateBrandingLogo();

            _bufferManager = new BufferManager(txtLogs, tsslBufferSize, tsslFlushCount, this);
            _deviceManager = new DeviceManager();
            _historyManager = new HistoryManager();

            // Initial history entry
            _historyManager.AddHistory(
                "Application",
                "Started");

            RefreshHistory();

             
            AttachEventHandlers();

            HideAllPanels();
            LoadConnectedDevices();
            StartStatsTimer();
            StartDiskIoMonitor();
            StartUptimeTimer();
            StartBufferTimer();
            StartDateTimeTimer();
            ShowDashboard();
        }

        // UI
        private void ApplyModernTheme()
        {
            this.BackColor = ContentBg;
            this.Font = new Font("Segoe UI", 9.75F);

            StyleHeaderPanel();
            StyleSidebar();

            StyleNavButton(btnDashboard);
            StyleNavButton(btnDevices);
            StyleNavButton(btnTransfers);
            StyleNavButton(btnLogs);
            StyleNavButton(btnReports);
            StyleNavButton(btnHistory);
            SetActiveNavButton(btnDashboard);

            pnlMain.BackColor = ContentBg;

            lblOverview.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblOverview.ForeColor = TextPrimary;
            lblOverview.Margin = new Padding(0, 0, 0, 4);

            StyleCard(grpDeviceCount);
            StyleCard(grpTransferCount);
            StyleCard(grpSuccessCount);
            StyleCard(grpFailedCount);

            StyleStatLabel(lblDeviceCount, AccentBlue);
            StyleStatLabel(lblTransferCount, AccentViolet);
            StyleStatLabel(lblSuccessCount, AccentGreen);
            StyleStatLabel(lblFailedCount, AccentAmber);

            StyleContentGroupBox(grpDeviceTable);
            StyleContentGroupBox(grpTransferActivity);

            StyleGrid(dgvDevices);
            StyleGrid(dgvTransfers);

            StyleStatusStrip();

            pnlStatistics.WrapContents = false;
            pnlStatistics.AutoScroll = false;
            pnlStatistics.BackColor = Color.Transparent;

            pnlSeparator.BackColor = BorderSubtle;

            dgvDevices.Dock = DockStyle.Fill;
            dgvTransfers.Dock = DockStyle.Fill;
        }

        private void StyleHeaderPanel()
        {
            pnlHeader.BackColor = CardBg;
            pnlHeader.Padding = new Padding(24, 16, 24, 16);
            pnlHeader.Paint += (s, e) =>
            {
                using (var pen = new Pen(BorderSubtle))
                    e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            };

            lblTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTitle.ForeColor = TextPrimary;
            lblTitle.AutoSize = true;

            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = TextMuted;
            lblSubtitle.AutoSize = true;

            lblDateTime.Font = new Font("Segoe UI", 10F);
            lblDateTime.ForeColor = TextMuted;
            lblDateTime.TextAlign = ContentAlignment.MiddleRight;
            lblDateTime.AutoSize = false;
            lblDateTime.Width = 260;
            lblDateTime.Height = 28;
            lblDateTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }

        private void StyleSidebar()
        {
            pnlNavigation.BackColor = SidebarBg;
            pnlNavigation.Padding = new Padding(12, 20, 12, 12);
            pnlNavigation.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(30, 41, 59)))
                    e.Graphics.DrawLine(pen, pnlNavigation.Width - 1, 0, pnlNavigation.Width - 1, pnlNavigation.Height);
            };
        }

        private void StyleNavButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = SidebarHover;
            btn.FlatAppearance.MouseDownBackColor = SidebarActive;
            btn.Height = 46;
            btn.Margin = new Padding(0, 0, 0, 6);
            btn.BackColor = SidebarBg;
            btn.ForeColor = Color.FromArgb(203, 213, 225);
            btn.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(16, 0, 0, 0);
            btn.UseVisualStyleBackColor = false;

            btn.MouseEnter += (s, e) =>
            {
                if (btn != _activeNavButton)
                    btn.BackColor = SidebarHover;
            };
            btn.MouseLeave += (s, e) =>
            {
                if (btn != _activeNavButton)
                    btn.BackColor = SidebarBg;
            };
        }

        private void SetActiveNavButton(Button active)
        {
            if (_activeNavButton != null)
            {
                _activeNavButton.BackColor = SidebarBg;
                _activeNavButton.ForeColor = Color.FromArgb(203, 213, 225);
            }

            _activeNavButton = active;
            _activeNavButton.BackColor = SidebarActive;
            _activeNavButton.ForeColor = Color.White;
        }

        private void StyleCard(GroupBox grp)
        {
            grp.BackColor = CardBg;
            grp.ForeColor = TextMuted;
            grp.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            grp.Padding = new Padding(16, 12, 16, 16);
            grp.Margin = new Padding(8, 0, 8, 0);
            if (!(grp is ModernCard))
                ApplyFlatGroupBoxPaint(grp);
        }

        private void StyleContentGroupBox(GroupBox grp)
        {
            grp.BackColor = CardBg;
            grp.ForeColor = TextPrimary;
            grp.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            grp.Padding = new Padding(16, 8, 16, 16);
            grp.Margin = new Padding(0, 0, 0, 16);
            if (!(grp is ModernCard))
                ApplyFlatGroupBoxPaint(grp);
        }

        private void ApplyFlatGroupBoxPaint(GroupBox grp)
        {
            grp.Paint += (s, e) =>
            {
                var box = (GroupBox)s;
                var titleHeight = (int)e.Graphics.MeasureString(box.Text, box.Font).Height;
                var bounds = new Rectangle(0, titleHeight / 2, box.Width - 1, box.Height - titleHeight / 2 - 1);
                using (var borderPen = new Pen(BorderSubtle))
                    e.Graphics.DrawRectangle(borderPen, bounds);
            };
        }

        private void StyleGrid(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = CardBg;
            grid.GridColor = BorderSubtle;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeight = 42;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersDefaultCellStyle.BackColor = GridHeaderBg;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 0, 8, 0);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.DefaultCellStyle.BackColor = CardBg;
            grid.DefaultCellStyle.ForeColor = TextPrimary;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            grid.DefaultCellStyle.Padding = new Padding(12, 4, 8, 4);
            grid.DefaultCellStyle.SelectionBackColor = GridSelectionBg;
            grid.DefaultCellStyle.SelectionForeColor = GridSelectionFg;
            grid.AlternatingRowsDefaultCellStyle.BackColor = GridAltRow;
            grid.AlternatingRowsDefaultCellStyle.ForeColor = TextPrimary;
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = GridSelectionBg;
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = GridSelectionFg;
            grid.RowTemplate.Height = 36;
            grid.RowHeadersVisible = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void StyleStatLabel(Label lbl, Color accentColor)
        {
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.AutoSize = false;
            lbl.Font = new Font("Segoe UI Semibold", 32F, FontStyle.Bold);
            lbl.ForeColor = accentColor;
        }

        private void StyleStatusStrip()
        {
            statusStrip1.BackColor = CardBg;
            statusStrip1.ForeColor = TextMuted;
            statusStrip1.Font = new Font("Segoe UI", 9F);
            statusStrip1.Padding = new Padding(12, 0, 8, 0);
            statusStrip1.SizingGrip = false;
            statusStrip1.RenderMode = ToolStripRenderMode.Professional;

            lblSystemStatus.ForeColor = AccentGreen;
            lblSystemStatus.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblSystemStatus.Margin = new Padding(4, 0, 12, 0);
        }

        private void StyleSecondaryButton(Button btn, Color backColor, Color foreColor, Color hoverColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = hoverColor;
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
            btn.Padding = new Padding(12, 0, 12, 0);
        }

        // =================================================================
        // WM_DEVICECHANGE
        // =================================================================
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg != WM_DEVICECHANGE) return;

            int eventType = m.WParam.ToInt32();

            if (eventType == DBT_DEVICEARRIVAL)
            {
                new System.Threading.Timer(_ =>
                {
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        int before = deviceCache.Count;
                        LoadConnectedDevices(forceRefresh: true);
                        int added = deviceCache.Count - before;
                        if (added > 0) { _sessionConnects += added;

                            AddDeviceLog($"▶ {added} device(s) connected");

                            _historyManager.AddHistory(
                                $"{added} device(s)", 
                                "Connected");
                            RefreshHistory();

                            RefreshReports(); }
                    }));
                }, null, 600, System.Threading.Timeout.Infinite);
            }
            else if (eventType == DBT_DEVICEREMOVECOMPLETE)
            {
                BeginInvoke((MethodInvoker)(() =>
                {
                    int before = deviceCache.Count;
                    LoadConnectedDevices(forceRefresh: true);
                    int removed = before - deviceCache.Count;
                    if (removed > 0) { _sessionDisconnects += removed; 

                        AddDeviceLog($"◀ {removed} device(s) disconnected"); 

                        _historyManager.AddHistory(
                            $"{removed} device(s)",
                            "Disconnected");
                        RefreshHistory();

                        RefreshReports(); }
                }));
            }
            else if (eventType == DBT_DEVNODES_CHANGED)
            {
                lock (_hidLock)
                {
                    if (_hidDebounceTimer == null)
                    {
                        _hidDebounceTimer = new System.Threading.Timer(_ =>
                        {
                            lock (_hidLock) { _hidDebounceTimer?.Dispose(); _hidDebounceTimer = null; }
                            BeginInvoke((MethodInvoker)(() =>
                            {
                                int before = deviceCache.Count;
                                LoadConnectedDevices(forceRefresh: true);
                                int delta = deviceCache.Count - before;
                                if (delta > 0) { _sessionConnects += delta;
                                    AddDeviceLog($"▶ {delta} HID device(s) connected");

                                    _historyManager.AddHistory(
                                        $"{delta} HID device(s)",
                                        "Connected");
                                    RefreshHistory();

                                    RefreshReports(); }

                                if (delta < 0) { _sessionDisconnects += -delta;
                                    AddDeviceLog($"◀ {-delta} HID device(s) disconnected");

                                    _historyManager.AddHistory(
                                        $"{-delta} HID device(s)",
                                        "Disconnected");
                                    RefreshHistory();

                                    RefreshReports(); }
                            }));
                        }, null, 800, System.Threading.Timeout.Infinite);
                    }
                    else
                    {
                        _hidDebounceTimer.Change(800, System.Threading.Timeout.Infinite);
                    }
                }
            }
        }

        // =================================================================
        // DEVICE LOADER
        // =================================================================
        private void LoadConnectedDevices(bool forceRefresh = false)
        {
            var current = new Dictionary<string, (string Name, uint ErrCode)>();

            try
            {
                using (var searcher = new ManagementObjectSearcher(
                    "SELECT Name, PNPDeviceID, ConfigManagerErrorCode " +
                    "FROM Win32_PnPEntity WHERE Present = TRUE"))
                {
                    foreach (ManagementObject device in searcher.Get())
                    {
                        string name = device["Name"]?.ToString();
                        string pnpId = device["PNPDeviceID"]?.ToString();
                        uint errCode = 0;
                        try { errCode = (uint)(device["ConfigManagerErrorCode"] ?? 0u); } catch { }

                        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(pnpId)) continue;
                        if (errCode != 0 && errCode != 22) continue;

                        string lower = name.ToLower();
                        string pnpLower = pnpId.ToLower();

                        bool isMouse = lower.Contains("hid-compliant mouse") || lower.Contains("usb mouse") ||
                                          (lower.Contains("mouse") && pnpLower.StartsWith("hid\\"));
                        bool isKeyboard = lower.Contains("hid-compliant keyboard") || lower.Contains("hid keyboard") ||
                                          lower.Contains("usb keyboard") ||
                                          (lower.Contains("keyboard") && !isMouse && pnpLower.StartsWith("hid\\"));
                        bool isExternal = isMouse || isKeyboard ||
                                          lower.Contains("usb input device") ||
                                          lower.Contains("usb mass storage") ||
                                          lower.Contains("usb audio") ||
                                          lower.Contains("usb composite") ||
                                          lower.Contains("usb hub") ||
                                          lower.Contains("usb device") ||
                                          lower.Contains("hid-compliant consumer") ||
                                          lower.Contains("hid-compliant game") ||
                                          lower.Contains("hid-compliant vendor") ||
                                          lower.Contains("usb disk") ||
                                          lower.Contains("usb storage");
                        bool isBluetooth = lower.Contains("bluetooth") &&
                                           !lower.Contains("serial") && !lower.Contains("enumerator") &&
                                           !lower.Contains("avrcp") && !lower.Contains("hands-free") &&
                                           !lower.Contains("audio sink") && !lower.Contains("remote control") &&
                                           !lower.Contains("le protocol") && !lower.Contains("rfcomm");
                        bool isNamedBtDevice = errCode == 0 && (
                                           lower.Contains("buds") || lower.Contains("airpods") ||
                                           lower.Contains("headset") || lower.Contains("headphone") ||
                                           lower.Contains("wh-") || lower.Contains("wf-") ||
                                           lower.Contains("jbl") || lower.Contains("jabra") ||
                                           lower.Contains("soundcore") || lower.Contains("anker") ||
                                           lower.Contains("bose") ||
                                           (lower.Contains("speaker") && lower.Contains("wireless")));
                        bool isWifi = (lower.Contains("wi-fi") || lower.Contains("wifi") ||
                                       lower.Contains("wireless") || lower.Contains("802.11") ||
                                       lower.Contains("wlan")) &&
                                      !lower.Contains("virtual") && !lower.Contains("miniport") &&
                                      !lower.Contains("tunnel");
                        bool isInternal =
                            lower.Contains("touchpad") || lower.Contains("synaptics") ||
                            lower.Contains("elan") || lower.Contains("alps") ||
                            lower.Contains("ps/2") || lower.Contains("standard ps/2") ||
                            lower.Contains("microsoft ps/2") || lower.Contains("i2c") ||
                            lower.Contains("system speaker") || lower.Contains("pci standard") ||
                            lower.Contains("wan miniport") || lower.Contains("microsoft ac adapter") ||
                            lower.Contains("amd smbus") || lower.Contains("plug and play software");

                        if ((isExternal || isBluetooth || isNamedBtDevice || isWifi) && !isInternal)
                            current[pnpId] = (name, errCode);
                    }
                }
            }
            catch (Exception ex) { AddDiskLog("WMI error: " + ex.Message); return; }

            current = DeduplicateBluetooth(current);
            current = DeduplicateCompositeChildren(current);
            current = DeduplicatePhysicalDevices(current);

            var currentIds = new HashSet<string>(current.Keys);
            var cachedIds = new HashSet<string>(deviceCache.Keys);
            if (!forceRefresh && currentIds.SetEquals(cachedIds)) return;

            var newCache = new Dictionary<string, (string Name, DateTime ConnectedAt, uint ErrCode)>();
            foreach (var kv in current)
            {
                DateTime connectedAt = deviceCache.ContainsKey(kv.Key)
                    ? deviceCache[kv.Key].ConnectedAt
                    : DateTime.Now;
                newCache[kv.Key] = (kv.Value.Name, connectedAt, kv.Value.ErrCode);
            }
            AddDiskLog("Current device count from WMI: " + current.Count);
            deviceCache = newCache;

            RebuildDeviceGrid();
            AddDiskLog($"Device list refreshed — {deviceCache.Count} device(s) found");
            UpdateStats();
        }

        private void RebuildDeviceGrid()
        {
            dgvDevices.Rows.Clear();
            string filterText = txtSearchDevices?.Text?.Trim()?.ToLower() ?? "";
            foreach (var kv in deviceCache)
            {
                string name = kv.Value.Name;
                string type = _deviceManager.ClassifyDevice(name.ToLower());
                if (!string.IsNullOrEmpty(filterText))
                {
                    if (!name.ToLower().Contains(filterText) && !type.ToLower().Contains(filterText))
                        continue;
                }
                uint errCode = kv.Value.ErrCode;
                string status = errCode == 22 ? "Disabled" : "Active";

                TimeSpan uptime = DateTime.Now - kv.Value.ConnectedAt;
                string uptimeStr = uptime.TotalMinutes < 1
                    ? $"{(int)uptime.TotalSeconds}s"
                    : uptime.TotalHours < 1
                        ? $"{(int)uptime.TotalMinutes}m {uptime.Seconds:D2}s"
                        : $"{(int)uptime.TotalHours}h {uptime.Minutes:D2}m";

                string xferRate = IsStorageType(type) ? "Idle" : "N/A";

                int rowIdx = dgvDevices.Rows.Add(name, type, status, xferRate, uptimeStr);

                var row = dgvDevices.Rows[rowIdx];
                if (errCode == 22)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 200);
                else
                    row.DefaultCellStyle.BackColor = Color.FromArgb(220, 255, 220);
            }
        }

        private bool IsStorageType(string type) =>
            type == "Storage" || type == "USB";

        private Dictionary<string, (string Name, uint ErrCode)> DeduplicateBluetooth(
            Dictionary<string, (string Name, uint ErrCode)> input)
        {
            var remove = new HashSet<string>();
            foreach (var kvLong in input)
                foreach (var kvShort in input)
                {
                    if (kvLong.Key == kvShort.Key) continue;
                    if (kvLong.Value.Name.StartsWith(kvShort.Value.Name, StringComparison.OrdinalIgnoreCase)
                        && kvLong.Value.Name.Length > kvShort.Value.Name.Length)
                    { remove.Add(kvLong.Key); break; }
                }
            return input.Where(kv => !remove.Contains(kv.Key))
                        .ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        // Removes composite/parent PnP stubs when a more-specific child node
        // for the same physical hardware is already in the set.
        // Rule: if pnpIdA is a strict prefix of pnpIdB, remove A.
        private Dictionary<string, (string Name, uint ErrCode)> DeduplicateCompositeChildren(
            Dictionary<string, (string Name, uint ErrCode)> input)
        {
            var keys = input.Keys.ToList();
            var remove = new HashSet<string>();

            for (int i = 0; i < keys.Count; i++)
            {
                string a = keys[i].ToUpperInvariant();
                for (int j = 0; j < keys.Count; j++)
                {
                    if (i == j) continue;
                    string b = keys[j].ToUpperInvariant();
                    // A is a parent stub of B — keep B (the more specific leaf), discard A
                    if (b.StartsWith(a) && b.Length > a.Length)
                    { remove.Add(keys[i]); break; }
                }
            }

            return input
                .Where(kv => !remove.Contains(kv.Key))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        private Dictionary<string, (string Name, uint ErrCode)> DeduplicatePhysicalDevices(
     Dictionary<string, (string Name, uint ErrCode)> input)
        {
            var result = new Dictionary<string, (string Name, uint ErrCode)>();

            var groups = input.GroupBy(kv =>
            {
                string n = kv.Value.Name.ToLower();

                if (n.Contains("keyboard"))
                    return "keyboard";

                if (n.Contains("mouse"))
                    return "mouse";

                if (n.Contains("airpods") ||
                    n.Contains("headset") ||
                    n.Contains("headphone") ||
                    n.Contains("speaker"))
                    return n;

                return kv.Key;
            });

            foreach (var group in groups)
            {
                var first = group.First();

                result[first.Key] = first.Value;
            }

            return result;
        }


        // =================================================================
        // UPTIME TIMER
        // =================================================================
        private void StartUptimeTimer()
        {
            uptimeTimer = new Timer { Interval = 30_000 };
            uptimeTimer.Tick += (s, e) =>
            {
                foreach (DataGridViewRow row in dgvDevices.Rows)
                {
                    string name = row.Cells["DeviceName"].Value?.ToString();
                    if (name == null) continue;
                    var entry = deviceCache.Values.FirstOrDefault(v => v.Name == name);
                    if (entry.Name == null) continue;

                    TimeSpan uptime = DateTime.Now - entry.ConnectedAt;
                    string uptimeStr = uptime.TotalMinutes < 1
                        ? $"{(int)uptime.TotalSeconds}s"
                        : uptime.TotalHours < 1
                            ? $"{(int)uptime.TotalMinutes}m {uptime.Seconds:D2}s"
                            : $"{(int)uptime.TotalHours}h {uptime.Minutes:D2}m";

                    row.Cells["Uptime"].Value = uptimeStr;
                }
            };
            uptimeTimer.Start();
        }

        // =================================================================
        // REAL DISK I/O MONITOR
        // =================================================================
        private void StartDiskIoMonitor()
        {
            PollDiskIo(priming: true);
            diskIoTimer = new Timer { Interval = 2000 };
            diskIoTimer.Tick += (s, e) => PollDiskIo(priming: false);
            diskIoTimer.Start();
        }

        private static ulong WmiUlong(object val)
        {
            if (val == null) return 0;
            try { return Convert.ToUInt64(val); } catch { return 0; }
        }

        private void PollDiskIo(bool priming)
        {
            UpdateCpuRam();
            try
            {
                using (var searcher = new ManagementObjectSearcher(
                    "SELECT Name, DiskReadBytesPersec, DiskWriteBytesPersec, " +
                    "DiskTransfersPersec, PercentDiskTime " +
                    "FROM Win32_PerfFormattedData_PerfDisk_LogicalDisk"))
                {
                    foreach (ManagementObject disk in searcher.Get())
                    {
                        string diskName = disk["Name"]?.ToString();
                        if (string.IsNullOrWhiteSpace(diskName) || diskName == "_Total") continue;

                        ulong readBps = WmiUlong(disk["DiskReadBytesPersec"]);
                        ulong writeBps = WmiUlong(disk["DiskWriteBytesPersec"]);
                        ulong transfers = WmiUlong(disk["DiskTransfersPersec"]);
                        ulong diskTime = WmiUlong(disk["PercentDiskTime"]);

                        if (priming) { prevReadBytes[diskName] = readBps; prevWriteBytes[diskName] = writeBps; continue; }

                        double readMBs = readBps / 1048576.0;
                        double writeMBs = writeBps / 1048576.0;
                        prevReadBytes[diskName] = readBps;
                        prevWriteBytes[diskName] = writeBps;

                        if (readMBs > _peakReadMBs) _peakReadMBs = readMBs;
                        if (writeMBs > _peakWriteMBs) _peakWriteMBs = writeMBs;

                        UpdateDeviceTransferRate(diskName, readMBs, writeMBs);

                        if (readBps == 0 && writeBps == 0) continue;

                        string ioMethod = DetermineIoMethod(readBps, writeBps, transfers, diskTime);

                        while (dgvTransfers.Rows.Count >= MAX_TRANSFER_ROWS)
                            dgvTransfers.Rows.RemoveAt(0);

                        string readStr = readMBs >= 0.01 ? $"{readMBs:F2} MB/s" : "< 0.01 MB/s";
                        string writeStr = writeMBs >= 0.01 ? $"{writeMBs:F2} MB/s" : "< 0.01 MB/s";
                        string activity = $"R: {readStr}  W: {writeStr}";

                        int rowIdx = dgvTransfers.Rows.Add(
                            diskName, ioMethod, activity,
                            $"{diskTime}% busy",
                            DateTime.Now.ToString("HH:mm:ss"));

                        if (diskTime > 80)
                            dgvTransfers.Rows[rowIdx].DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 180);

                        if (dgvTransfers.Rows.Count > 0)
                            dgvTransfers.FirstDisplayedScrollingRowIndex = dgvTransfers.Rows.Count - 1;

                        AddDiskLog($"[Disk {diskName}] {ioMethod} — {activity}");
                    }
                }
            }
            catch (Exception ex) { AddDiskLog("Disk I/O poll error: " + ex.Message); }

            UpdateStats();
        }

        private string DetermineIoMethod(ulong readBps, ulong writeBps, ulong transfers, ulong diskTime)
        {
            ulong totalBps = readBps + writeBps;
            if (transfers > 200 && totalBps < 5_000_000) return "Interrupt-Driven I/O";
            if (totalBps > 10_000_000) return "DMA";
            if (transfers > 0 && totalBps < 1_000_000) return "Programmed I/O";
            return "DMA";
        }

        private void UpdateDeviceTransferRate(string diskName, double readMBs, double writeMBs)
        {
            foreach (DataGridViewRow row in dgvDevices.Rows)
            {
                string devName = row.Cells["DeviceName"].Value?.ToString()?.ToLower() ?? "";
                if (devName.Contains("disk") || devName.Contains("drive") || devName.Contains("storage"))
                {
                    double total = readMBs + writeMBs;
                    row.Cells["TransferRate"].Value = total >= 0.01 ? $"{total:F2} MB/s" : "Idle";
                    break;
                }
            }
        }

        // =================================================================
        // STATS TIMER
        // =================================================================
        private void StartStatsTimer()
        {
            statsTimer = new Timer { Interval = 3000 };
            statsTimer.Tick += (s, e) => { LoadConnectedDevices(); UpdateStats(); };
            statsTimer.Start();
        }

        private void UpdateStats()
        {
            lblDeviceCount.Text = deviceCache.Count.ToString();
            lblTransferCount.Text = dgvTransfers.Rows.Count.ToString();

            int active = 0, idle = 0;
            foreach (DataGridViewRow r in dgvTransfers.Rows)
            {
                string act = r.Cells["Activity"]?.Value?.ToString() ?? "";
                bool isIdle = act == "" || (act.Contains("< 0.01") && act.LastIndexOf("< 0.01") != act.IndexOf("< 0.01"));
                if (isIdle) idle++; else active++;
            }

            grpSuccessCount.Text = "Active Transfers";
            grpFailedCount.Text = "Idle Polls";
            lblSuccessCount.Text = active.ToString();
            lblFailedCount.Text = idle.ToString();
        }

        // =================================================================
        // DATE/TIME TIMER
        // =================================================================
        private void StartDateTimeTimer()
        {
            lblDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            dateTimeTimer = new Timer { Interval = 1000 };
            dateTimeTimer.Tick += (s, e) =>
                lblDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            dateTimeTimer.Start();
        }

        // =================================================================
        // LOGGING
        // =================================================================
        private void AddDeviceLog(string msg)
        {
            if (txtLogs == null || txtLogs.IsDisposed) return;
            _bufferManager.BufferLog(msg);
            if (txtReports != null && !txtReports.IsDisposed)
                txtReports.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\r\n");
        }

        private void AddDiskLog(string msg)
        {
            if (txtLogs == null || txtLogs.IsDisposed) return;
            _bufferManager.BufferLog(msg);
        }

        // =================================================================
        // I/O BUFFER
        // =================================================================
        private void StartBufferTimer()
        {
            _bufferTimer = new Timer { Interval = 15_000 };
            _bufferTimer.Tick += (s, e) => _bufferManager.FlushBuffer(reason: "automatic");
            _bufferTimer.Start();
        }

        // =================================================================
        // REPORTS
        // =================================================================
        private void RefreshReports()
        {
            if (txtReports == null || txtReports.IsDisposed) return;

            TimeSpan uptime = DateTime.Now - _sessionStart;
            string summary =
                $"══════════════════════════════════════\r\n" +
                $"  PulseIO  —  Session Report\r\n" +
                $"  {DateTime.Now:MM/dd/yyyy HH:mm:ss}\r\n" +
                $"══════════════════════════════════════\r\n" +
                $"  Session uptime   : {uptime:hh\\:mm\\:ss}\r\n" +
                $"  Devices now      : {deviceCache.Count}\r\n" +
                $"  Total connects   : {_sessionConnects}\r\n" +
                $"  Total disconnects: {_sessionDisconnects}\r\n" +
                $"  Peak disk read   : {_peakReadMBs:F2} MB/s\r\n" +
                $"  Peak disk write  : {_peakWriteMBs:F2} MB/s\r\n" +
                $"  Transfer rows    : {dgvTransfers.Rows.Count}\r\n" +
                $"══════════════════════════════════════\r\n" +
                $"  Device Event Log\r\n" +
                $"──────────────────────────────────────\r\n";

            string existing = txtReports.Text;
            int divider = existing.IndexOf("──────────────────────────────────────");
            string events = divider >= 0
                ? existing.Substring(divider + "──────────────────────────────────────\r\n".Length)
                : "";

            txtReports.Text = summary + events;
        }

        private void SaveReport()
        {
            using (var dlg = new SaveFileDialog
            {
                Title = "Save PulseIO Session Report",
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                FileName = $"PulseIO_Report_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
            })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(dlg.FileName, txtReports.Text);
                    AddDeviceLog($"Report saved: {Path.GetFileName(dlg.FileName)}");
                }
            }
        }

        private void RefreshHistory()
        {
            if (txtHistory == null || txtHistory.IsDisposed)
                return;

            txtHistory.Clear();

            foreach (var entry in _historyManager.GetHistory())
            {
                txtHistory.AppendText(
                    $"[{entry.Timestamp:MM/dd/yyyy HH:mm:ss}] " +
                    $"{entry.EventType} - {entry.DeviceName}" +
                    Environment.NewLine);
            }
        }

        private void ClearHistory()
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to clear all history entries? This cannot be undone.",
                "Clear History",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            _historyManager.ClearHistory();
            RefreshHistory();
        }

        // =================================================================
        // TAB NAVIGATION
        // =================================================================
        private void HideAllPanels()
        {
            lblOverview.Visible = false;
            pnlStatistics.Visible = false;
            grpDeviceTable.Visible = false;
            grpTransferActivity.Visible = false;
            txtLogs.Visible = false;
            txtReports.Visible = false;
            txtHistory.Visible = false;
            btnSaveReport.Visible = false;
            btnClearHistory.Visible = false;
            if (btnExportHistory != null) btnExportHistory.Visible = false;
            if (grpSystemHealth != null) grpSystemHealth.Visible = false;
        }

        private void ShowDashboard()
        {
            HideAllPanels();
            SetActiveNavButton(btnDashboard);
            lblOverview.Visible = true;
            pnlStatistics.Visible = true;
            if (grpSystemHealth != null) grpSystemHealth.Visible = true;
            grpDeviceTable.Visible = true;
            grpTransferActivity.Visible = true;
            
            grpDeviceTable.Dock = DockStyle.Top;
            grpTransferActivity.Dock = DockStyle.Top;
            if (grpSystemHealth != null) grpSystemHealth.Dock = DockStyle.Top;

            lblOverview.BringToFront();
            pnlStatistics.BringToFront();
            if (grpSystemHealth != null) grpSystemHealth.BringToFront();
            pnlSeparator.BringToFront();
            grpDeviceTable.BringToFront();
            grpTransferActivity.BringToFront();
        }

        private void ShowDevices()
        {
            HideAllPanels();
            SetActiveNavButton(btnDevices);
            grpDeviceTable.Dock = DockStyle.Fill;
            grpDeviceTable.Visible = true;
        }

        private void ShowTransfers()
        {
            HideAllPanels();
            SetActiveNavButton(btnTransfers);
            grpTransferActivity.Dock = DockStyle.Fill;
            grpTransferActivity.Visible = true;
        }

        private void ShowLogs()
        {
            HideAllPanels();
            SetActiveNavButton(btnLogs);
            txtLogs.Visible = true;
        }

        private void ShowReports()
        {
            HideAllPanels();
            SetActiveNavButton(btnReports);
            RefreshReports();
            txtReports.Visible = true;
            btnSaveReport.Visible = true;
        }

        private void ShowHistory()
        {
            HideAllPanels();
            SetActiveNavButton(btnHistory);
            RefreshHistory();
            txtHistory.Visible = true;
            btnClearHistory.Visible = true;
            if (btnExportHistory != null) btnExportHistory.Visible = true;
        }

        // =================================================================
        // INITIALIZATION
        // =================================================================
        private void InitializeDataGridViewColumns()
        {
            dgvDevices.Columns.Add("DeviceName", "Device Name");
            dgvDevices.Columns.Add("DeviceType", "Device Type");
            dgvDevices.Columns.Add("Status", "Status");
            dgvDevices.Columns.Add("TransferRate", "Transfer Rate");
            dgvDevices.Columns.Add("Uptime", "Connected For");

            dgvTransfers.Columns.Add("Disk", "Disk");
            dgvTransfers.Columns.Add("IoMethod", "I/O Method");
            dgvTransfers.Columns.Add("Activity", "Read / Write");
            dgvTransfers.Columns.Add("DiskBusy", "Disk Busy");
            dgvTransfers.Columns.Add("Timestamp", "Timestamp");
        }

        private void CreateLogsAndReportsControls()
        {
            // ── Logs Container ──────────────────────────────────────────────
            pnlLogsContainer = new Panel { Dock = DockStyle.Fill, Visible = false };
            pnlLogsWrapper = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.FromArgb(15, 23, 42),
                BorderColor = Color.FromArgb(30, 41, 59),
                CornerRadius = 10,
                Padding = new Padding(12)
            };
            txtLogs = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10F),
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.FromArgb(134, 239, 172),
                BorderStyle = BorderStyle.None
            };
            pnlLogsWrapper.Controls.Add(txtLogs);
            pnlLogsContainer.Controls.Add(pnlLogsWrapper);
            pnlMain.Controls.Add(pnlLogsContainer);

            // ── Reports Container ───────────────────────────────────────────
            pnlReportsContainer = new Panel { Dock = DockStyle.Fill, Visible = false };
            pnlReportsWrapper = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.White,
                BorderColor = Color.FromArgb(226, 232, 240),
                CornerRadius = 10,
                Padding = new Padding(12)
            };
            txtReports = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10F),
                BackColor = CardBg,
                ForeColor = TextPrimary,
                BorderStyle = BorderStyle.None
            };
            pnlReportsWrapper.Controls.Add(txtReports);

            pnlReportsBottom = new Panel { Dock = DockStyle.Bottom, Height = 54, Padding = new Padding(0, 8, 0, 8) };
            btnSaveReport = new ModernButton
            {
                Text = "Save Report",
                Size = new Size(140, 38),
                Dock = DockStyle.Right
            };
            StyleSecondaryButton(btnSaveReport, GridHeaderBg, Color.White, SidebarHover);
            btnSaveReport.Click += (s, e) => SaveReport();
            pnlReportsBottom.Controls.Add(btnSaveReport);

            pnlReportsContainer.Controls.Add(pnlReportsWrapper);
            pnlReportsContainer.Controls.Add(pnlReportsBottom);
            pnlMain.Controls.Add(pnlReportsContainer);

            // ── History Container ───────────────────────────────────────────
            pnlHistoryContainer = new Panel { Dock = DockStyle.Fill, Visible = false };
            pnlHistoryWrapper = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.White,
                BorderColor = Color.FromArgb(226, 232, 240),
                CornerRadius = 10,
                Padding = new Padding(12)
            };
            txtHistory = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10F),
                BackColor = CardBg,
                ForeColor = TextPrimary,
                BorderStyle = BorderStyle.None
            };
            pnlHistoryWrapper.Controls.Add(txtHistory);

            pnlHistoryBottom = new Panel { Dock = DockStyle.Bottom, Height = 54, Padding = new Padding(0, 8, 0, 8) };

            btnClearHistory = new ModernButton
            {
                Text = "Clear History",
                Size = new Size(140, 38),
                Dock = DockStyle.Right
            };
            StyleSecondaryButton(btnClearHistory, GridHeaderBg, Color.White, SidebarHover);
            btnClearHistory.Click += (s, e) => ClearHistory();

            Panel pnlSpacer = new Panel { Dock = DockStyle.Right, Width = 12 };

            btnExportHistory = new ModernButton
            {
                Text = "Export History",
                Size = new Size(140, 38),
                Dock = DockStyle.Right
            };
            StyleSecondaryButton(btnExportHistory, GridHeaderBg, Color.White, SidebarHover);
            btnExportHistory.Click += (s, e) => ExportHistory();

            pnlHistoryBottom.Controls.Add(btnExportHistory);
            pnlHistoryBottom.Controls.Add(pnlSpacer);
            pnlHistoryBottom.Controls.Add(btnClearHistory);

            pnlHistoryContainer.Controls.Add(pnlHistoryWrapper);
            pnlHistoryContainer.Controls.Add(pnlHistoryBottom);
            pnlMain.Controls.Add(pnlHistoryContainer);
    }
        private void CreateStatusBarControls()
        {
            // Buffer stats — live counters in the status bar (always visible, no panel needed)
            tsslBufferSize = new System.Windows.Forms.ToolStripStatusLabel
            {
                Text = "Buffer: 0",
                ForeColor = TextMuted,
                BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left,
                BorderStyle = Border3DStyle.Flat,
                Margin = new System.Windows.Forms.Padding(12, 0, 8, 0),
                Font = new Font("Segoe UI", 9F)
            };

            tsslFlushCount = new System.Windows.Forms.ToolStripStatusLabel
            {
                Text = "Flushes: 0",
                ForeColor = TextMuted,
                BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left,
                BorderStyle = Border3DStyle.Flat,
                Margin = new System.Windows.Forms.Padding(8, 0, 0, 0),
                Font = new Font("Segoe UI", 9F)
            };

            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                tsslBufferSize,
                tsslFlushCount
            });
        }

        private void AttachEventHandlers()
        {
            btnDashboard.Click += (s, e) => ShowDashboard();
            btnDevices.Click += (s, e) => ShowDevices();
            btnTransfers.Click += (s, e) => ShowTransfers();
            btnLogs.Click += (s, e) => ShowLogs();
            btnReports.Click += (s, e) => ShowReports();
            btnHistory.Click += (s, e) => ShowHistory();
            dgvDevices.CellDoubleClick += DgvDevices_CellDoubleClick;
        }

        // =================================================================
        // CLEANUP
        // =================================================================
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            lock (_hidLock) { _hidDebounceTimer?.Dispose(); _hidDebounceTimer = null; }
            statsTimer?.Stop();
            diskIoTimer?.Stop();
            uptimeTimer?.Stop();
            _bufferTimer?.Stop();
            dateTimeTimer?.Stop();
            try { cpuCounter?.Dispose(); } catch { }
            base.OnFormClosing(e);
        }

        // ── Custom Setup & Additions ──────────────────────────────────────
        private void InitializePerformanceCounters()
        {
            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                cpuCounter.NextValue();
            }
            catch { }
        }

        private void CreateCpuRamCards()
        {
            grpCpuUsage = new ModernCard
            {
                Text = "CPU Usage",
                Size = new Size(221, 120),
                Margin = new Padding(8, 0, 8, 0)
            };
            lblCpuUsage = new Label();
            grpCpuUsage.Controls.Add(lblCpuUsage);
            StyleCard(grpCpuUsage);
            StyleStatLabel(lblCpuUsage, AccentViolet);

            grpRamUsage = new ModernCard
            {
                Text = "RAM Usage",
                Size = new Size(221, 120),
                Margin = new Padding(8, 0, 8, 0)
            };
            lblRamUsage = new Label();
            grpRamUsage.Controls.Add(lblRamUsage);
            StyleCard(grpRamUsage);
            StyleStatLabel(lblRamUsage, AccentBlue);
            lblRamUsage.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);

            pnlStatistics.Controls.Add(grpCpuUsage);
            pnlStatistics.Controls.Add(grpRamUsage);

            pnlStatistics.WrapContents = true;
            pnlStatistics.AutoScroll = true;
            pnlStatistics.Height = 260;
        }

        private void CreateSearchControls()
        {
            lblSearchDevices = new Label
            {
                Text = "Search Devices:",
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = TextMuted,
                Size = new Size(110, 20),
                Location = new Point(580, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            txtSearchDevices = new TextBox
            {
                Font = new Font("Segoe UI", 9.5F),
                Size = new Size(180, 25),
                Location = new Point(700, 22),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            
            txtSearchDevices.TextChanged += (s, e) => RebuildDeviceGrid();

            pnlHeader.Controls.Add(lblSearchDevices);
            pnlHeader.Controls.Add(txtSearchDevices);
        }

        private void CreateSystemHealthSection()
        {
            grpSystemHealth = new ModernCard
            {
                Text = "System Health Status",
                Height = 85,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 0, 0, 16)
            };
            StyleContentGroupBox(grpSystemHealth);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            var pnlCpu = CreateHealthPanel("CPU LOAD", out lblHealthCpuVal, AccentViolet);
            var pnlRam = CreateHealthPanel("RAM USAGE", out lblHealthRamVal, AccentBlue);
            var pnlDevs = CreateHealthPanel("CONNECTED DEVICES", out lblHealthDevicesVal, AccentGreen);
            var pnlXfers = CreateHealthPanel("ACTIVE TRANSFERS", out lblHealthTransfersVal, AccentAmber);

            layout.Controls.Add(pnlCpu, 0, 0);
            layout.Controls.Add(pnlRam, 1, 0);
            layout.Controls.Add(pnlDevs, 2, 0);
            layout.Controls.Add(pnlXfers, 3, 0);

            grpSystemHealth.Controls.Add(layout);
            pnlMain.Controls.Add(grpSystemHealth);
        }

        private Panel CreateHealthPanel(string title, out Label valLabel, Color valColor)
        {
            var pnl = new Panel { Dock = DockStyle.Fill };
            
            var lblTitleText = new Label
            {
                Text = title,
                Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold),
                ForeColor = TextMuted,
                Location = new Point(10, 10),
                AutoSize = true
            };

            valLabel = new Label
            {
                Text = "--",
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = valColor,
                Location = new Point(10, 26),
                AutoSize = true
            };

            pnl.Controls.Add(lblTitleText);
            pnl.Controls.Add(valLabel);
            return pnl;
        }

        private void PositionBottomButtons()
        {
            int rightMargin = 24;
            int bottomMargin = 24;
            int buttonHeight = 38;
            int buttonWidth = 140;

            int y = pnlMain.ClientSize.Height - buttonHeight - bottomMargin;
            int xClear = pnlMain.ClientSize.Width - buttonWidth - rightMargin;
            int xExport = xClear - buttonWidth - 12; 
            int xSaveReport = pnlMain.ClientSize.Width - buttonWidth - rightMargin;

            if (btnClearHistory != null) btnClearHistory.Location = new Point(xClear, y);
            if (btnExportHistory != null) btnExportHistory.Location = new Point(xExport, y);
            if (btnSaveReport != null) btnSaveReport.Location = new Point(xSaveReport, y);
        }

        private void UpdateCpuRam()
        {
            try
            {
                // CPU
                float cpuVal = 0;
                if (cpuCounter != null)
                {
                    cpuVal = cpuCounter.NextValue();
                }
                lblCpuUsage.Text = $"{cpuVal:F0}%";
                lblHealthCpuVal.Text = $"{cpuVal:F0}%";

                // RAM
                var computerInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
                ulong totalPhys = computerInfo.TotalPhysicalMemory;
                ulong availPhys = computerInfo.AvailablePhysicalMemory;
                ulong usedPhys = totalPhys - availPhys;

                double totalGB = totalPhys / (1024.0 * 1024.0 * 1024.0);
                double usedGB = usedPhys / (1024.0 * 1024.0 * 1024.0);
                double ramUsagePercent = (double)usedPhys / totalPhys * 100.0;

                lblRamUsage.Text = $"{usedGB:F1} / {totalGB:F1} GB";
                lblHealthRamVal.Text = $"{ramUsagePercent:F0}%";

                // Connected Devices
                lblHealthDevicesVal.Text = deviceCache.Count.ToString();

                // Active Transfers
                int active = 0;
                foreach (DataGridViewRow r in dgvTransfers.Rows)
                {
                    string act = r.Cells["Activity"]?.Value?.ToString() ?? "";
                    bool isIdle = act == "" || (act.Contains("< 0.01") && act.LastIndexOf("< 0.01") != act.IndexOf("< 0.01"));
                    if (!isIdle) active++;
                }
                lblHealthTransfersVal.Text = active.ToString();
            }
            catch { }
        }

        private void DgvDevices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvDevices.Rows[e.RowIndex];
            string deviceName = row.Cells["DeviceName"].Value?.ToString();
            if (string.IsNullOrEmpty(deviceName)) return;

            var cachedEntry = deviceCache.FirstOrDefault(kv => kv.Value.Name == deviceName);
            string pnpId = cachedEntry.Key ?? "N/A";
            DateTime connectedAt = cachedEntry.Value.ConnectedAt != default 
                ? cachedEntry.Value.ConnectedAt 
                : DateTime.Now;
            uint errCode = cachedEntry.Value.ErrCode;
            string status = errCode == 22 ? "Disabled" : "Active";
            string deviceType = row.Cells["DeviceType"].Value?.ToString() ?? "Unknown";

            string manufacturer = "Unknown";
            if (pnpId != "N/A")
            {
                try
                {
                    string escapedId = pnpId.Replace("\\", "\\\\");
                    using (var searcher = new ManagementObjectSearcher(
                        $"SELECT Manufacturer FROM Win32_PnPEntity WHERE PNPDeviceID = '{escapedId}'"))
                    {
                        foreach (ManagementObject device in searcher.Get())
                        {
                            manufacturer = device["Manufacturer"]?.ToString() ?? "Unknown";
                        }
                    }
                }
                catch { }
            }

            using (var detailsForm = new DeviceDetailsForm(deviceName, deviceType, status, connectedAt, pnpId, manufacturer))
            {
                detailsForm.ShowDialog(this);
            }
        }

        private void ExportHistory()
        {
            using (var dlg = new SaveFileDialog
            {
                Title = "Export History Log",
                Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt|All files (*.*)|*.*",
                FileName = $"PulseIO_History_{DateTime.Now:yyyyMMdd_HHmmss}"
            })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string ext = Path.GetExtension(dlg.FileName).ToLower();
                        var history = _historyManager.GetHistory();
                        if (ext == ".csv")
                        {
                            var sb = new StringBuilder();
                            sb.AppendLine("Timestamp,DeviceName,EventType");
                            foreach (var entry in history)
                            {
                                string name = EscapeCsv(entry.DeviceName);
                                string evt = EscapeCsv(entry.EventType);
                                sb.AppendLine($"{entry.Timestamp:yyyy-MM-dd HH:mm:ss},{name},{evt}");
                            }
                            File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
                        }
                        else
                        {
                            var sb = new StringBuilder();
                            foreach (var entry in history)
                            {
                                sb.AppendLine($"[{entry.Timestamp:MM/dd/yyyy HH:mm:ss}] {entry.EventType} - {entry.DeviceName}");
                            }
                            File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
                        }
                        AddDeviceLog($"History successfully exported to {Path.GetFileName(dlg.FileName)}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to export history: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string EscapeCsv(string val)
        {
            if (string.IsNullOrEmpty(val)) return "";
            if (val.Contains(",") || val.Contains("\"") || val.Contains("\n") || val.Contains("\r"))
            {
                return $"\"{val.Replace("\"", "\"\"")}\"";
            }
            return val;
        }

        private void CreateBrandingLogo()
        {
            var pnlBranding = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(16, 10, 16, 10)
            };

            pnlBranding.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Draw pulse line logo in neon Indigo
                using (var pen = new Pen(Color.FromArgb(99, 102, 241), 2))
                {
                    var points = new Point[]
                    {
                        new Point(16, 40),
                        new Point(30, 40),
                        new Point(35, 20),
                        new Point(40, 60),
                        new Point(45, 35),
                        new Point(50, 45),
                        new Point(55, 40),
                        new Point(70, 40)
                    };
                    g.DrawLines(pen, points);
                }

                // Draw glow
                using (var penGlow = new Pen(Color.FromArgb(40, 99, 102, 241), 4))
                {
                    var points = new Point[]
                    {
                        new Point(16, 40),
                        new Point(30, 40),
                        new Point(35, 20),
                        new Point(40, 60),
                        new Point(45, 35),
                        new Point(50, 45),
                        new Point(55, 40),
                        new Point(70, 40)
                    };
                    g.DrawLines(penGlow, points);
                }

                // Title
                using (var font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold))
                using (var brush = new SolidBrush(Color.White))
                {
                    g.DrawString("PulseIO", font, brush, new PointF(80, 24));
                }
            };

            pnlNavigation.Controls.Add(pnlBranding);
            pnlBranding.BringToFront();

            // Re-stack nav buttons under branding
            btnDashboard.BringToFront();
            btnDevices.BringToFront();
            btnTransfers.BringToFront();
            btnLogs.BringToFront();
            btnReports.BringToFront();
            btnHistory.BringToFront();
        }
    }
}