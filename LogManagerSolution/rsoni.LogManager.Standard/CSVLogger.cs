using rsoni.LogManager.Standard.Common;
using rsoni.LogManager.Standard.ExceptionTypes;
using rsoni.LogManager.Standard.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rsoni.LogManager.Standard
{
    public class CSVLogger : BaseLogger, ILogger
    {
        #region Fields
        //
        #endregion Fields

        public CSVLogger(string logName) : base(logName)
        {
            // Do Nothing.
        }
        public CSVLogger(IConfiguration configuration) : base(configuration)
        {
            //After base create telematry object.
        }

        public CSVLogger(string logFileName, string logError, string logInfo, string logWarning, string logTrack)
            : base(logError, logInfo, logWarning, logTrack)
        {
            this.LogFileName = LogFileName;
        }

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
                    LogEntry2(new CSVFileLogMessage()
                    {
                        CorrelationId = this.CorrelationId,
                        LogType = Enums.LogType.Error,
                        Message = message
                    });
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
                    LogEntry2(new CSVFileLogMessage()
                    {
                        CorrelationId = this.CorrelationId,
                        LogType = Enums.LogType.Error,
                        Message = exception.Message,
                        ExceptionDetails = exception
                    });
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
        private void LogEntry2(CSVFileLogMessage logMsg)
        {
            string msg = string.Empty;
            try
            {
                FileLock.AcquireWriterLock(60000);
                string logFile = GetLogFolder() + this.LogFileName + ".csv";
                if (File.Exists(logFile))
                {
                    DateTime creationDateTime = File.GetCreationTime(logFile);
                    // System.TimeSpan diff = DateTime.Now.Subtract(creationDateTime);
                    //if (diff.Days > 1)
                    // Check if just the Date is changed so that even at 11 pm in night will go in different file.
                    if (DateTime.Now.ToString("yyyyMMdd") != creationDateTime.ToString("yyyyMMdd"))
                    {
                        File.Copy(logFile, GetLogFolder() + this.LogFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");
                        // This logic is just to set creation date and time for file.
                        File.Delete(logFile);
                    }
                    msg = logMsg.ToString();
                }
                else
                {
                    string headingLine = logMsg.ToHeadingString() + Environment.NewLine;
                    msg = headingLine + logMsg.ToString();
                }

                bool setCreationTime = false;

                Object thisLock = new Object();
                lock (thisLock)
                {
                    if (File.Exists(logFile) == false)
                        setCreationTime = true;
                    StreamWriter logWriter = new StreamWriter(logFile, true);
                    logWriter.WriteLine(msg);
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
                LogEntry2(new CSVFileLogMessage()
                {
                    CorrelationId = this.CorrelationId,
                    LogType = Enums.LogType.Information,
                    Message = message
                });
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
                if (IsLogTrack)
                {
                    var executionTime = endTime - startTime;
                    string formattedMessage = String.Format("Controller:{0}. Method:{1}"
                                        , controllerName, methodName);
                    LogEntry2(new CSVFileLogMessage()
                    {
                        CorrelationId = this.CorrelationId,
                        LogType = Enums.LogType.Track,
                        StartDateTime = startTime,
                        EndDateTime = endTime,
                        UserId = userId,
                        Message = formattedMessage
                    });
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
                LogEntry2(new CSVFileLogMessage()
                {
                    CorrelationId = this.CorrelationId,
                    LogType = Enums.LogType.Warning,
                    Message = message
                });
            }
        }

        public void TrackApplicationEvent(Guid correlationId, Enums.ApplicationEvent eventName, Dictionary<string, string> properties)
        {
            try
            {
                if (IsLogTrack)
                {
                    LogEntry2(new CSVFileLogMessage()
                    {
                        CorrelationId = this.CorrelationId,
                        EventType = eventName,
                        LogType = Enums.LogType.Track,
                        Message = properties.ToDebugString()
                    });
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
            try
            {
                if (IsLogTrack)
                {
                    LogEntry2(new CSVFileLogMessage()
                    {
                        CorrelationId = this.CorrelationId,
                        LogType = Enums.LogType.Track,
                        Message = eventName.ToString() + " - " + properties.ToDebugString()
                    });
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

        public void TrackSystemEvent(Guid correlationId, Enums.EnrollmentEvent eventName, Dictionary<string, string> properties)
        {
            try
            {
                if (IsLogTrack)
                {
                    LogEntry2(new CSVFileLogMessage()
                    {
                        CorrelationId = correlationId,
                        LogType = Enums.LogType.Track,
                        Message = eventName.ToString() + " - " + properties.ToDebugString()
                    });
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
    }


    public class CSVFileLogMessage
    {

        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public DateTime CurrentTime { get; set; } = DateTime.Now;

        public string UserId { get; set; } = "System";

        public Enums.LogType LogType { get; set; } = Enums.LogType.None;

        public Enums.ApplicationEvent EventType { get; set; } = Enums.ApplicationEvent.None;

        public string Message { get; set; } = string.Empty;

        public Exception ExceptionDetails { get; set; } = null;

        public DateTime StartDateTime { get; set; } = DateTime.Now;

        public DateTime EndDateTime { get; set; } = DateTime.Now;


        public double TotalMilliSeconds
        {
            get
            {
                var executionTime = EndDateTime - StartDateTime;
                double returnValue = executionTime.TotalMilliseconds;
                return returnValue;

            }

        }

        public override string ToString()
        {
            string Msg = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
                , this.CorrelationId.ToString()
                , this.UserId
                , this.CurrentTime.ToString("yyyyMMdd-HHmmss-fff")
                , this.LogType.ToString()
                , this.EventType.ToString()
                , this.StartDateTime.ToString("yyyyMMdd-HHmmss-fff")
                , this.EndDateTime.ToString("yyyyMMdd-HHmmss-fff")
                , this.TotalMilliSeconds.ToString("###0.000000")
                , this.Message.Replace(",", ".#.")
                , (this.ExceptionDetails == null) ? "Null" : this.ExceptionDetails.Message.Replace(",", ".#.")
                );
            return Msg;
        }


        public string ToHeadingString()
        {
            string Msg = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}"
                , "CorrelationId"
                , "UserId"
                , "CurrentTime"
                , "LogType"
                , "EventType"
                , "StartDateTime"
                , "EndDateTime"
                , "TotalMilliSeconds"
                , "Message"
                , "ExceptionDetails"
                );
            return Msg;
        }


    }

}
