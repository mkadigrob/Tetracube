using System;
using System.IO;
using System.Text;

namespace Logger.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var logService = new LogService();
            logService.AddLogger(CreateFileLogger());

            while (true)
            {
                logService.Debug("Debug message");
                logService.Trace("Trace message");
                logService.Info("Info message");
                logService.Warning("Warning message");
                logService.Error("Error message");
                logService.Error("Error message", new Exception("Exception message"));
                logService.Error("Error message", new Exception("Exception message", new Exception("Inner exception message")));

                Console.WriteLine("Log saved (Exit - press Esc, Repeat - press any other key)");
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Escape)
                    break;
            }
        }

        static ILogger CreateFileLogger()
        {
            Action<string> archivator = fileName =>
            {
                var archiveFileName = Path.Combine(
                    Path.GetDirectoryName(fileName),
                    Path.GetFileNameWithoutExtension(fileName) + DateTime.UtcNow.ToString("_yyyyMMdd_HHmmssfff") + Path.GetExtension(fileName));

                File.Copy(fileName, archiveFileName);
            };

            var logger = new FileLogger("Test.log", 1000, Encoding.UTF8, archivator);

            logger.RegisterMessage<DebugLogMessage>(msg => $"{msg.Timestamp:dd-MM-yyyy HH:mm:ss.fff} [DEBUG]: {msg.Message}");

            logger.Error += (s, e) =>
            {
                var msg = $"[{e.Exception.GetType().Name}] {e.Exception.Message}";

                if (e.Exception.InnerException != null)
                    msg += $" ({e.Exception.InnerException.Message})";

                Console.WriteLine(msg);
            };

            return logger;
        }
    }
}
