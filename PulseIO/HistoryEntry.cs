using System;

namespace PulseIO
{
    public class HistoryEntry
    {
        public DateTime Timestamp { get; set; }

        public string DeviceName { get; set; }

        public string EventType { get; set; }
    }
}