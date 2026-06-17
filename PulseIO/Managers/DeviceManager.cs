using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseIO
{
    internal class DeviceManager
    {
        public string ClassifyDevice(string lower)
        {
            if (lower.Contains("mouse")) return "Mouse";
            if (lower.Contains("keyboard")) return "Keyboard";
            if (lower.Contains("disk") || lower.Contains("drive") || lower.Contains("storage"))
                return "Storage";
            if (lower.Contains("audio")) return "Audio";
            if (lower.Contains("wi-fi") || lower.Contains("wifi") ||
                lower.Contains("wireless") || lower.Contains("wlan") || lower.Contains("802.11"))
                return "Wi-Fi";
            if (lower.Contains("bluetooth") || lower.Contains("buds") ||
                lower.Contains("wh-") || lower.Contains("wf-") ||
                lower.Contains("airpods") || lower.Contains("jbl") ||
                lower.Contains("headphone") || lower.Contains("headset"))
                return "Bluetooth";
            if (lower.Contains("hid")) return "HID";
            if (lower.Contains("usb")) return "USB";
            return "I/O Device";
        }
    }
}
