using Logger.Messages;

namespace Logger
{
    public interface ILogger
    {
        void Log(LogMessage message);
    }
}
