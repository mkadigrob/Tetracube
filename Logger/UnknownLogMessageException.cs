using System;

namespace Logger
{
    public class UnknownLogMessageException : Exception
    {
        public UnknownLogMessageException(Type logMessageType) : base($"Неизвестный тип сообщения {logMessageType.Name}") 
        {
            LogMessageType = logMessageType;
        }

        public Type LogMessageType { get; }
    }
}
