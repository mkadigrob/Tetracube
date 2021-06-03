using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;


/// <summary>
///   Дан файл примитивного логгера для плагина (Logger.cs).
///
///1. По текущему коду указать, что с ним не так. Ответ "всё" правильный, но недостаточный.
///
///2. Предложить свой вариант реализации логирования с записью в локальный файл сообщений с разным уровнем критичности. 
///   При превышении размера журнала некоторой константы, старый файл архивируется и заводится новый. 
///   Предусмотреть возможность последующего расширения функциональности.
/// </summary>
namespace Logger
{
    public class Logger
    {
        private readonly DirectoryInfo path; // зачем использовать этот тип?
        private readonly string name;
        private StreamWriter log;

        private string logAccum = "";

        private const long maxSize = 31457280; // из названия константы непонятная еденица измерения и размер чего она представляет

        public Logger(string path, string name = "RevitPlugins.log")
        {
            this.path = new DirectoryInfo(path);

            if (!this.path.Exists)
                createFolder();

            this.name = name;
            log = null;
        }

        ~Logger()
        {
            close(); 
        }

        private bool open()
        {
            //rotate();

            try
            {
                log = new StreamWriter(path + name, true, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                print("Failed to create log file '" + name + "'");
                print(ex.Message);
                return false;
            }

            log.AutoFlush = true;

            return true;
        }

        private bool rotate()
        {
            if (!File.Exists(path + name))
            {
                print("Failed to open log file '" + name + "'");
                return false;
            }

            var currentLogSize = new FileInfo(path + name).Length;

            if (currentLogSize >= maxSize)
            {
                string content;

                using (var logFileReader = new StreamReader(path + name, Encoding.UTF8))
                {
                    content = logFileReader.ReadToEnd();
                    logFileReader.Close();
                }

                var logFileWriter = new StreamWriter(path + name, false, Encoding.UTF8);

                for (int i = content.Length / 2; i < content.Length; i++)
                    logFileWriter.Write(content[i]);

                logFileWriter.Close();
            }

            return true;
        }

        private void print(string message)
        {
            //TaskDialog.Show("Error", message);
        }

        private void close()
        {
            log?.Close();
        }

        private bool createFolder()
        {
            try
            {
                path.Create();
                return true;
            }
            catch (Exception ex)
            {
                print(ex.Message);
                print("Failed to create the folder \"" + path + "\"");
                return false;
            }
        }

        public void GetMessageBatch(List<String> messages, string message) {
            messages.ForEach(p => { logAccum += p; });           
        }


        public void LOG_NONE(string message, bool msgbox = false)
        {
            var timestamp = DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            var str = timestamp + " [TRACE  ]: " + message;

            if (!open()) return;
            log?.WriteLine(str);
            close();

            if (msgbox)
                print(str);
        }

        public void LOG_INFO(string message, bool msgbox = false)
        {
            var timestamp = DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            var str = timestamp + " [INFO   ]: " + message;

            if (!open()) return;
            log?.WriteLine(str);
            close();

            if (msgbox)
                print(str);
        }

        public void LOG_DEBUG(string message, bool msgbox = false)
        {
            var timestamp = DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            var str = timestamp + " [DEBUG  ]: " + message;

            if (!open()) return;
            log?.WriteLine(str);
            close();

            if (msgbox)
                print(str);
        }

        public void LOG_WARNING(string message, bool msgbox = false)
        {
            var timestamp = DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            var str = timestamp + " [WARNING]: " + message;

            if (!open()) return;
            log?.WriteLine(str);
            close();

            if (msgbox)
                print(str);
        }

        public void LOG_ERROR(string message, bool msgbox = false)
        {
            var timestamp = DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            var str = timestamp + " [ERROR  ]: " + message;

            if (!open()) return;
            log?.WriteLine(str);
            close();

            if (msgbox)
                print(str);
        }
    };
}
