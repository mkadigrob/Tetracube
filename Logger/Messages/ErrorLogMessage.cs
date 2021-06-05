using System;

namespace Logger.Messages
{
    public class ErrorLogMessage : LogMessage
    {
        public ErrorLogMessage(string message) : base(message) { }

        public ErrorLogMessage(string message, Exception exception) : base(message) 
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }
}
