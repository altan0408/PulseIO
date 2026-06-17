using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
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
        private Button btnSaveReport;

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

        // =================================================================
        public MainForm()
        {
            InitializeComponent();
            ApplyModernTheme();
            InitializeDataGridViewColumns();
            CreateLogsAndReportsControls();
            CreateStatusBarControls();

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
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Font = new Font("Segoe UI", 9F);

            pnlHeader.BackColor = Color.White;

            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 41, 59);

            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);

            lblDateTime.ForeColor = Color.FromArgb(100, 116, 139);
            lblDateTime.TextAlign = ContentAlignment.MiddleRight;
            lblDateTime.AutoSize = false;
            lblDateTime.Width = 250;
            lblDateTime.Height = 25;
            lblDateTime.Left = pnlHeader.Width - 270;
            lblDateTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            lblTitle.AutoSize = true;
            lblSubtitle.AutoSize = true;

            pnlNavigation.BackColor = Color.FromArgb(15, 23, 42);

            StyleNavButton(btnDashboard);
            StyleNavButton(btnDevices);
            StyleNavButton(btnTransfers);
            StyleNavButton(btnLogs);
            StyleNavButton(btnReports);

            lblOverview.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblOverview.ForeColor = Color.FromArgb(30, 41, 59);

            StyleCard(grpDeviceCount);
            StyleCard(grpTransferCount);
            StyleCard(grpSuccessCount);
            StyleCard(grpFailedCount);

            CenterStatLabel(lblDeviceCount);
            CenterStatLabel(lblTransferCount);
            CenterStatLabel(lblSuccessCount);
            CenterStatLabel(lblFailedCount);

            StyleGrid(dgvDevices);
            StyleGrid(dgvTransfers);

            statusStrip1.BackColor = Color.White;

            pnlStatistics.WrapContents = false;
            pnlStatistics.AutoScroll = false;

            grpDeviceTable.Padding = new Padding(10);
            grpTransferActivity.Padding = new Padding(10);

            dgvDevices.Dock = DockStyle.Fill;
            dgvTransfers.Dock = DockStyle.Fill;
        }

        private void StyleNavButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Height = 55;
            btn.BackColor = Color.FromArgb(15, 23, 42);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(15, 0, 0, 0);
        }

        private void StyleCard(GroupBox grp)
        {
            grp.BackColor = Color.White;
            grp.ForeColor = Color.FromArgb(30, 41, 59);
            grp.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }

        private void StyleGrid(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = Color.White;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            grid.RowTemplate.Height = 30;
            grid.RowHeadersVisible = false;
        }

        private void CenterStatLabel(Label lbl)
        {
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
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
            foreach (var kv in deviceCache)
            {
                string name = kv.Value.Name;
                string type = _deviceManager.ClassifyDevice(name.ToLower());
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
        }

        private void ShowDashboard()
        {
            HideAllPanels();
            lblOverview.Visible = true;
            pnlStatistics.Visible = true;
            grpDeviceTable.Visible = true;
            grpTransferActivity.Visible = true;
            grpDeviceTable.Dock = DockStyle.Top;
            grpTransferActivity.Dock = DockStyle.Top;
        }

        private void ShowDevices()
        {
            HideAllPanels();
            grpDeviceTable.Dock = DockStyle.Fill;
            grpDeviceTable.Visible = true;
        }

        private void ShowTransfers()
        {
            HideAllPanels();
            grpTransferActivity.Dock = DockStyle.Fill;
            grpTransferActivity.Visible = true;
        }

        private void ShowLogs()
        {
            HideAllPanels();
            txtLogs.Visible = true;
        }

        private void ShowReports()
        {
            HideAllPanels();
            RefreshReports();
            txtReports.Visible = true;
            btnSaveReport.Visible = true;
        }

        private void ShowHistory()
        {
            HideAllPanels();

            RefreshHistory();

            txtHistory.Visible = true;
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
            txtLogs = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9f),
                BackColor = Color.Black,
                ForeColor = Color.LimeGreen
            };
            pnlMain.Controls.Add(txtLogs);

            txtReports = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9f),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            pnlMain.Controls.Add(txtReports);


            txtHistory = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9f),
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            pnlMain.Controls.Add(txtHistory);

            btnSaveReport = new Button
            {
                Text = "💾  Save Report",
                Size = new Size(130, 32),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Visible = false,
                BackColor = Color.FromArgb(30, 30, 60),
                ForeColor = Color.Cyan,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f)
            };
            btnSaveReport.Click += (s, e) => SaveReport();
            pnlMain.Controls.Add(btnSaveReport);
            btnSaveReport.BringToFront();
        }

        private void CreateStatusBarControls()
        {
            // Buffer stats — live counters in the status bar (always visible, no panel needed)
            tsslBufferSize = new System.Windows.Forms.ToolStripStatusLabel
            {
                Text = "Buffer: 0",
                ForeColor = System.Drawing.Color.DimGray,
                BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left,
                Margin = new System.Windows.Forms.Padding(8, 0, 4, 0)
            };

            tsslFlushCount = new System.Windows.Forms.ToolStripStatusLabel
            {
                Text = "Flushes: 0",
                ForeColor = System.Drawing.Color.DimGray,
                BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left,
                Margin = new System.Windows.Forms.Padding(4, 0, 0, 0)
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
            base.OnFormClosing(e);
        }
    }
}