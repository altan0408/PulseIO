using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PulseIO
{
    internal class BufferManager
    {
        private readonly Queue<string> _logBuffer = new Queue<string>();
        private readonly object _bufferLock = new object();
        private const int BUFFER_FLUSH_THRESHOLD = 10;
        private int _bufferFlushCount = 0;

        private readonly TextBox _txtLogs;
        private readonly ToolStripStatusLabel _tsslBufferSize;
        private readonly ToolStripStatusLabel _tsslFlushCount;
        private readonly Form _form;

        public BufferManager(TextBox txtLogs, ToolStripStatusLabel tsslBufferSize, ToolStripStatusLabel tsslFlushCount, Form form)
        {
            _txtLogs = txtLogs;
            _tsslBufferSize = tsslBufferSize;
            _tsslFlushCount = tsslFlushCount;
            _form = form;
        }

        public void BufferLog(string msg)
        {
            string entry = $"[{DateTime.Now:HH:mm:ss}] {msg}";

            bool shouldFlush = false;
            lock (_bufferLock)
            {
                _logBuffer.Enqueue(entry);
                if (_logBuffer.Count >= BUFFER_FLUSH_THRESHOLD)
                    shouldFlush = true;
            }

            if (shouldFlush)
                FlushBuffer(reason: "threshold");
        }

        public void FlushBuffer(string reason)
        {
            if (_txtLogs == null || _txtLogs.IsDisposed) return;

            if (_form.InvokeRequired)
            {
                _form.BeginInvoke((MethodInvoker)(() => FlushBuffer(reason)));
                return;
            }

            string[] lines;
            int count;

            lock (_bufferLock)
            {
                count = _logBuffer.Count;
                if (count == 0) return;

                lines = _logBuffer.ToArray();
                _logBuffer.Clear();
                _bufferFlushCount++;
            }

            var sb = new StringBuilder();

            string reasonLabel = reason == "threshold"
                ? $"Flushed {count} entries"
                : "Automatic flush executed";

            sb.AppendLine($"[BUFFER] ── {reasonLabel} ──────────────────");
            foreach (string line in lines)
                sb.AppendLine(line);
            sb.AppendLine($"[BUFFER] ── End flush #{_bufferFlushCount} ─────────────────");
            sb.AppendLine();

            _txtLogs.AppendText(sb.ToString());
            UpdateBufferStats();
        }

        private void UpdateBufferStats()
        {
            if (_tsslBufferSize == null || _tsslFlushCount == null) return;
            int currentSize;
            lock (_bufferLock) { currentSize = _logBuffer.Count; }
            _tsslBufferSize.Text = $"Buffer: {currentSize}";
            _tsslFlushCount.Text = $"Flushes: {_bufferFlushCount}";
        }
    }
}
