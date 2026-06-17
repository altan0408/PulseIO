using System.Collections.Generic;

namespace PulseIO
{
    internal class HistoryManager
    {
        private readonly List<HistoryEntry> _history =
            new List<HistoryEntry>();

        public void AddHistory(string deviceName, string eventType)
        {
            _history.Add(new HistoryEntry
            {
                Timestamp = System.DateTime.Now,
                DeviceName = deviceName,
                EventType = eventType
            });
        }

        public List<HistoryEntry> GetHistory()
        {
            return new List<HistoryEntry>(_history);
        }

        public void ClearHistory()
        {
            _history.Clear();
        }

        public int Count
        {
            get { return _history.Count; }
        }
    }
}