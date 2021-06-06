using System;

namespace Logger
{
    public class ArchiveLogException : Exception
    {
        public ArchiveLogException(Exception innerException) : base("Ошибка при архивировании лога", innerException) { }
    }
}
