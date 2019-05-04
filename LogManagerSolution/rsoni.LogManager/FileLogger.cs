using rsoni.LogManager.Common;
using rsoni.LogManager.ExceptionTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rsoni.LogManager
{
    public class FileLogger : ILogger
    {
        #region Fields
        private IConfiguration Configuration;
        private bool IsLogError;
        private bool IsLogInfo;
        private bool IsLogWarn;
        private bool IsLogTrack;
        private string logFileName;
        #endregion Fields

        public FileLogger(string logName)
        {
            Configuration = new Configuration();
            IsLogError = Configuration.GetAppSettingEntry<Boolean>(Constants.AppSettingsKeys.Logging.LogError);
            IsLogInfo = Configuration.GetAppSettingEntry<Boolean>(Constants.AppSettingsKeys.Logging.LogInfo);
            IsLogWarn = Configuration.GetAppSettingEntry<Boolean>(Constants.AppSettingsKeys.Logging.LogWarning);
            IsLogTrack = Configuration.GetAppSettingEntry<Boolean>(Constants.AppSettingsKeys.Logging.LogTrack);
            logFileName = logName;
        }

        public Guid CorrelationId { get; set; } = Guid.NewGuid();

        public string GetDetailsfromLogger(DateTime startDatetime, DateTime endDateTime)
        {
            throw new NotImplementedException();
        }

        public void LogEntry(Enums.LogType logType, string message)
        {
            LogEntry(logType, message, null);
        }

        public void LogEntry(Enums.LogType logType, string message, Exception exception)
        {
            switch (logType)
            {
                case Enums.LogType.Error:
                    if (exception == null)
                        LogError(message);
                    else
                        LogError(exception);
                    break;

                case Enums.LogType.Warning:
                    LogWarning(message);
                    break;

                case Enums.LogType.Information:
                    LogInfo(message);
                    break;

                default:
                    LogInfo(message);
                    break;
            }
        }

        public void LogError(string message)
        {
            try
            {
                if (IsLogError)
                {
                    LogEntry2("Error :" + message);
                }
            }
            catch (LogMangerException)
            {
                throw;
            }
            catch (Exception innerException)
            {
                throw new LogMangerException(nameof(ErrorCodeMessages.LoggerError106) + ": " + ErrorCodeMessages.LoggerError106, innerException);
            }
        }

        public void LogError(Exception exception)
        {
            try
            {
                if (IsLogError)
                {
                    Dictionary<string, string> exceptionDictionary = new Dictionary<string, string>();
                    foreach (DictionaryEntry exceptionData in exception.Data)
                    {
                        exceptionDictionary.Add(exceptionData.Key.ToString(), exceptionData.Value.ToString());
                    }
                    LogEntry2("Exception :" + exception.StackTrace);
                }
            }
            catch (LogMangerException)
            {
                throw;
            }
            catch (Exception)
            {
                // Empty catch, as it is used by global execption handler to log error. If some
                // exception is thrown from this method it will result in endless loop.
            }
        }

        /// <summary>
        /// Get LogFolder path
        /// </summary>
        /// <returns></returns>
        private string GetLogFolder()
        {
            string logFolder = @"C:\logs\";
            if (string.IsNullOrEmpty(logFolder))
            {
                //  logFolder = System.IO.Directory.GetCurrentDirectory();
                logFolder = AppDomain.CurrentDomain.BaseDirectory;
            }

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }
            return logFolder;
        }

        static ReaderWriterLock FileLock = new ReaderWriterLock();
        /// <summary>
        /// Log entry method to add one entry in log file.
        /// </summary>
        /// <param name="logMsg"></param>
        private void LogEntry2(string logMsg)
        {
            try
            {
                FileLock.AcquireWriterLock(60000);
                logMsg = "Time:" + DateTime.Now.ToShortTimeString() + " - " + logMsg;
                string logFile = GetLogFolder() + logFileName + ".log";
                if (File.Exists(logFile))
                {
                    DateTime creationDateTime = File.GetCreationTime(logFile);
                    // System.TimeSpan diff = DateTime.Now.Subtract(creationDateTime);
                    //if (diff.Days > 1)
                    // Check if just the Date is changed so that even at 11 pm in night will go in different file.
                    if (DateTime.Now.ToString("yyyyMMdd") != creationDateTime.ToString("yyyyMMdd"))
                    {
                        File.Copy(logFile, GetLogFolder() + logFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");
                        // This logic is just to set creation date and time for file.
                        File.Delete(logFile);
                    }
                }

                bool setCreationTime = false;

                Object thisLock = new Object();
                lock (thisLock)
                {
                    if (File.Exists(logFile) == false)
                        setCreationTime = true;
                    StreamWriter logWriter = new StreamWriter(logFile, true);
                    logWriter.WriteLine(logMsg);
                    logWriter.Flush();
                    logWriter.Close();
                    logWriter.Dispose();
                    if (setCreationTime)
                        File.SetCreationTime(logFile, DateTime.Now);
                }
                FileLock.ReleaseWriterLock();
            }
            catch (Exception ex)
            {
                //
            }

        }

        public void LogInfo(string message)
        {
            if (IsLogInfo)
            {
                LogEntry2("Info :" + message);
            }
        }

        public void LogMethodEnd(params string[] parameters)
        {
            throw new NotImplementedException();
        }

        public void LogMethodStart(params string[] parameters)
        {
            throw new NotImplementedException();
        }

        public void LogRequestDetails(string userId, DateTime startTime, DateTime endTime, string controllerName, string methodName)
        {
            try
            {
                if (IsLogInfo)
                {
                    var executionTime = endTime - startTime;
                    string formattedMessage = String.Format("UserId - {0}. Correlation Id - {1}. Controller Name - {2}. Method Name - {3}. Method call started at - {4}. Method call end at - {5}. Method execution time - {6} ", string.IsNullOrEmpty(userId) ? "Not Available" : userId, CorrelationId, controllerName, methodName, startTime, endTime, executionTime);
                    LogEntry2("LogRequestDetails :" + formattedMessage);
                }
            }
            catch (Exception ex)
            {
                throw new LogMangerException(nameof(ErrorCodeMessages.LoggerError106) + ": " + ErrorCodeMessages.LoggerError106, ex);
            }
        }

        public void LogWarning(string message)
        {
            if (IsLogWarn)
            {
                LogEntry2("Warning :" + message);
            }
        }

        public void TrackApplicationEvent(Guid correlationId, Enums.ApplicationEvent eventName, Dictionary<string, string> properties)
        {
            try
            {
                if (IsLogTrack)
                {
                    LogEntry2("TrackApplicationEvent :" + eventName + ", " + properties.ToDebugString());
                }
            }
            catch (LogMangerException)
            {
                throw;
            }
            catch (Exception innerException)
            {
                throw new LogMangerException(nameof(ErrorCodeMessages.LoggerError106) + ": " + ErrorCodeMessages.LoggerError106, innerException);
            }
        }

        public void TrackEvent(Enums.EnrollmentEvent eventName, Dictionary<string, string> properties = null)
        {
            throw new NotImplementedException();
        }

        public void TrackSystemEvent(Guid correlationId, Enums.EnrollmentEvent eventName, Dictionary<string, string> properties)
        {
            throw new NotImplementedException();
        }
    }
}
