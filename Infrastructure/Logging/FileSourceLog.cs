using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace PostcardApp.Infrastructure.Logging
{
    /// <summary>
    /// Implementation of contract 
    /// using System.Diagnostics API.
    /// </summary>
    public sealed class FileSourceLog : ILogger
    {
        #region Members

        static int expiryDays;
        static string logPath;
        static FileStream stream;
        static StreamWriter writer;
        static TraceEventType logLevel;

        #endregion

        #region  Constructor

        /// <summary>
        /// Create a new instance of this file manager
        /// </summary>
        public FileSourceLog()
        {
            // Create default source
            expiryDays = -30;
            logPath = @"C:\CodeRepo\GitHub\PostcardApp\Logs\";
            logLevel = TraceEventType.Information;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// File internal message in configured listeners
        /// </summary>
        /// <param name="eventType">Event type to File</param>
        /// <param name="message">Message of event</param>
        void FileInternal(TraceEventType eventType, string message, Exception exception)
        {
            if (eventType <= logLevel)
            {
                try
                {
                    if (IsDirectoryExists(logPath, true))
                    {
                        Open(logPath);
                        WriteLog(eventType.ToString(), message, exception);
                    }
                }
                catch (Exception)
                {
                    // TODO
                }
                finally
                {
                    Close();
                    Delete();
                }
            }
        }

        bool IsDirectoryExists(string directory, bool create)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    if (create)
                    {
                        Directory.CreateDirectory(directory);
                        return true;
                    }
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        void Open(string directory)
        {
            try
            {
                string fileName = directory + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                stream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                writer = new StreamWriter(stream);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        void WriteLog(string entryType, string message, Exception exception)
        {
            string log = "=============================================================" + Environment.NewLine;
            log += "LOCAL DATE  : " + DateTime.Now.ToString("dddd, MMMM dd yyyy, hh:mm:ss.ffff tt") + Environment.NewLine;
            log += "ENTRY TYPE  : " + entryType + Environment.NewLine;
            log += "SOURCE      : " + (exception == null ? "N/A" : exception.Source) + Environment.NewLine;
            log += "MESSAGE     : " + (message ?? exception.Message) + Environment.NewLine;
            log += "STACK TRACE : " + (exception == null ? "N/A" : exception.StackTrace) + Environment.NewLine;
            writer.WriteLine(log);
        }

        void Close()
        {
            try
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
            }
            catch (Exception)
            {
                // TODO
            }
        }

        void Delete()
        {
            ThreadPool.QueueUserWorkItem(o => DeletingLogs(logPath));
        }

        void DeletingLogs(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles("????-??-??.log");
            foreach (var file in files)
            {
                if (file.CreationTime < DateTime.Now.AddDays(expiryDays))
                {
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
            }
        }

        #endregion

        #region ILogger Members

        public void Info(string message, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var messageToFile = string.Format(CultureInfo.InvariantCulture, message, args);

                FileInternal(TraceEventType.Information, messageToFile, null);
            }
        }

        public void Warn(string message, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var messageToFile = string.Format(CultureInfo.InvariantCulture, message, args);

                FileInternal(TraceEventType.Warning, messageToFile, null);
            }
        }

        public void Error(string message, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var messageToFile = string.Format(CultureInfo.InvariantCulture, message, args);

                FileInternal(TraceEventType.Error, messageToFile, null);
            }
        }

        public void Error(Exception ex, params object[] args)
        {
            if (!string.IsNullOrWhiteSpace(ex.Message) && ex != null)
            {
                var messageToFile = string.Format(CultureInfo.InvariantCulture, ex.Message, args);

                FileInternal(TraceEventType.Error, messageToFile, ex);
            }
        }

        #endregion
    }
}