﻿using LastBell.Utilities;
using System.Windows.Threading;

namespace LastBell.Helpers
{
    public class BaseInactivityHelper
    {
        public BaseInactivityHelper(int waitTime)
        {
            _inactivityTime = waitTime;
            _timer.Tick += TimerOnTick;
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Start();
        }

        private readonly int _inactivityTime;
        private readonly DispatcherTimer _timer = new();
        public event InactivityEvent? OnInactivity;
        public delegate void InactivityEvent(int inactivityTime);

        private void TimerOnTick(object? sender, EventArgs e)
        {
            if (UserInactivity.GetSeconds() < _inactivityTime || UserInactivity.GetSeconds() > _inactivityTime) return;
            OnInactivity?.Invoke(UserInactivity.GetSeconds());
        }
    }
}