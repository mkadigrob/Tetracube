﻿using Logger.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logger
{
    public class FileLogger : ILogger
    {
        private readonly Dictionary<Type, Func<LogMessage, string>> _converters = new Dictionary<Type, Func<LogMessage, string>>();
        private readonly FileInfo _file;


        public FileLogger(string fileName, long maxFileSize, Encoding encoding)
        {
            _file = new FileInfo(fileName);
            MaxFileSize = maxFileSize;
            Encoding = encoding;

            InitConverters();
        }


        public long MaxFileSize { get; }

        public long FileSize
        {
            get
            {
                _file.Refresh();

                if (_file.Exists)
                    return _file.Length;

                return 0;
            }
        }

        public Encoding Encoding { get; }


        public event EventHandler<LoggerErrorEventArgs> Error;


        private void RaiseError(Exception exception)
        {
            if (Error == null)
                throw exception;

            Error?.Invoke(this, new LoggerErrorEventArgs(exception));
        }

        private void InitConverters()
        {
            Func<string, LogMessage, string> defaultConverter =
                (level, msg) => $"{msg.Timestamp:dd-MM-yyyy HH:mm:ss.fff} [{level}]: {msg.Message}";

            RegisterMessage<TraceLogMessage>(msg => defaultConverter("TRACE", msg));
            RegisterMessage<InfoLogMessage>(msg => defaultConverter("INFO", msg));
            RegisterMessage<WarningLogMessage>(msg => defaultConverter("WARNING", msg));
            RegisterMessage<ErrorLogMessage>(msg =>
            {
                var logMsg = defaultConverter("ERROR", msg);

                if (msg.Exception != null)
                {
                    var shift = "\n\t";
                    var exception = msg.Exception;

                    while (exception != null)
                    {
                        logMsg += $"{shift}[{exception.GetType().Name}] {exception.Message}";
                        exception = exception.InnerException;
                        shift += "\t";
                    }
                }

                return logMsg;
            });
        }

        private string ConvertToString(LogMessage message)
        {
            if (_converters.TryGetValue(message.GetType(), out Func<LogMessage, string> converter))
                return converter(message);

            throw new UnknownLogMessageException(message.GetType());
        }

        private void Write(string message, bool truncate)
        {
            var fileMode = truncate ? FileMode.Truncate : FileMode.Append;

            using (var sw = new StreamWriter(_file.Open(fileMode, FileAccess.Write), Encoding))
            {
                sw.WriteLine(message);
            }
        }

        private string GetArchiveFileName()
        {
            return Path.Combine(
                _file.DirectoryName,
                Path.GetFileNameWithoutExtension(_file.Name) + DateTime.Now.ToString("_yyyyMMdd_HHmmssfff") + Path.GetExtension(_file.Name));
        }

        private void Archive()
        {
            var archiveFileName = GetArchiveFileName();
            try
            {
                _file.CopyTo(archiveFileName);
            }
            catch (Exception ex)
            {
                throw new ArchiveLogException(archiveFileName, ex);
            }
        }

        public void RegisterMessage<T>(Func<T, string> converter) where T : LogMessage
        {
            _converters[typeof(T)] = msg => converter((T)msg);
        }

        public void UnregisterMessage<T>() where T : LogMessage
        {
            _converters.Remove(typeof(T));
        }

        public void Log(LogMessage message)
        {
            try
            {
                var truncate = FileSize >= MaxFileSize;

                if (truncate)
                    Archive();

                Write(ConvertToString(message), truncate);
            }
            catch (Exception ex)
            {
                RaiseError(ex);
            }
        }
    }
}
