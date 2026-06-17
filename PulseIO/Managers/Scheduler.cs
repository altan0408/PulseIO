using System;
using System.Windows.Forms;

namespace PulseIO
{
    internal class Scheduler
    {
        public Timer CreateTimer(int interval, EventHandler tickEvent)
        {
            Timer timer = new Timer();

            timer.Interval = interval;

            timer.Tick += tickEvent;

            timer.Start();

            return timer;
        }
    }
}