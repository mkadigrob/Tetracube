using Logger.Messages;

namespace Logger.App
{
    public class DebugLogMessage : LogMessage
    {
        public DebugLogMessage(string message) : base(message) { }
    }
}
