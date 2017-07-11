using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace HandySyncService
{
    class Logger
    {
        private static NLog.Logger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Writes the diagnostic message at the Trace level.
        /// </summary>
        /// <param name="message">Log message</param>
        public void Trace(string message)
        {
            Console.WriteLine("[TRACE]: {0}", message);
            log.Trace(message);
            WriteToFile("[TRACE]: " + message);
        }

        /// <summary>
        /// Writes the diagnostic message at the Info level.
        /// </summary>
        /// <param name="message">Log message</param>
        public void Info(string message)
        {
            Console.WriteLine("[INFO]:  {0}", message);
            log.Info(message);
            WriteToFile("[INFO]: " + message);
        }
        
        /// <summary>
        /// Writes the diagnostic message at the Debug level.
        /// </summary>
        /// <param name="message">Log message</param>
        public void Debug(string message)
        {
            Console.WriteLine("[DEBUG]: {0}", message);
            log.Debug(message);
            WriteToFile("[DEBUG]: " + message);
        }
        
        /// <summary>
        /// Writes the diagnostic message at the Warning level.
        /// </summary>
        /// <param name="message">Log message</param>
        public void Warn(string message)
        {
            Console.WriteLine("[WARN]:  {0}", message);
            log.Warn(message);
            WriteToFile("[WARN]: " + message);
        }

        /// <summary>
        /// Writes the diagnostic message at the Error level.
        /// </summary>
        /// <param name="message">Log message</param>
        public void Error(string message)
        {
            Console.WriteLine("[ERROR]: {0}", message);
            log.Error(message);
            WriteToFile("[ERROR]: " + message);
        }

        /// <summary>
        /// Writes the diagnostic message at the Fatal level.
        /// </summary>
        /// <param name="message">Log message</param>
        public void Fatal(string message)
        {
            Console.WriteLine("[FATAL]: {0}", message);
            log.Fatal(message);
            WriteToFile("[FATAL]: " + message);
        }

        //public static StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);

        public static void WriteToFile(string message)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Log Files\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            fileInfo.Directory.Create(); // If the directory already exists, this method does nothing.
            using (StreamWriter file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Log Files\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", true))
            {
                file.WriteLine(DateTime.Now + " : " + message);
            }
        }
    }
}
