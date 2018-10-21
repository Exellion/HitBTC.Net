using HitBTC.Net.Models;
using System;

namespace HitBTC.Net.Communication
{
    public class HitEventArgs : EventArgs
    {
        public HitConnectionState ConnectionState { get; private set; }

        public Exception SocketError { get; private set; }

        public HitNotification Notification { get; private set; }

        public HitReport Report { get; private set; }

        internal HitEventArgs(HitConnectionState connectionState) => this.ConnectionState = connectionState;

        internal HitEventArgs(HitConnectionState connectionState, Exception socketError)
            : this(connectionState) => this.SocketError = socketError;

        internal HitEventArgs(HitNotification hitNotification) => this.Notification = hitNotification;

        internal HitEventArgs(HitReport hitReport) => this.Report = hitReport;
    }
}