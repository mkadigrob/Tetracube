using System;

namespace Logger
{
    public class ArchiveLogException : Exception
    {
        public ArchiveLogException(string message, Exception innerException) : base(message, innerException) { }
    }
}
