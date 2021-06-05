using System;

namespace Logger.Messages
{
    public abstract class LogMessage
    {
        public LogMessage(string message)
        {
            Timestamp = DateTime.Now;
            Message = message;
        }

        public DateTime Timestamp { get; protected set; }
        public string Message { get; protected set; }
    }
}
