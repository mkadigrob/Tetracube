using Logger.Messages;
using System;
using System.Collections.Generic;

namespace Logger.App
{
    class LogService
    {
        private List<ILogger> _loogers = new List<ILogger>();

        private void Log(LogMessage message)
        {
            _loogers.ForEach(logger => logger.Log(message));
        }

        public void AddLogger(ILogger logger)
        {
            _loogers.Add(logger);
        }

        public void Debug(string message)
        {
            Log(new DebugLogMessage(message));
        }

        public void Trace(string message)
        {
            Log(new TraceLogMessage(message));
        }

        public void Info(string message)
        {
            Log(new InfoLogMessage(message));
        }

        public void Warning(string message)
        {
            Log(new WarningLogMessage(message));
        }

        public void Error(string message)
        {
            Log(new ErrorLogMessage(message));
        }

        public void Error(string message, Exception exception)
        {
            Log(new ErrorLogMessage(message, exception));
        }
    }
}
