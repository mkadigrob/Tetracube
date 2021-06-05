using System;

namespace Logger
{
    public class LoggerErrorEventArgs : EventArgs
    {
        public LoggerErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }
}
