
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using rsoni.LogManager.Common;
using rsoni.LogManager.ExceptionTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
namespace rsoni.LogManager
{

    public class AppInsightLogger : BaseLogger, ILogger
    {
        #region Fields
        private string logFileName = "AppInsightLogger";
        internal TelemetryClient TelemetryClient;
        #endregion Fields

        #region Constructor

        

        public AppInsightLogger(string logName) : base(logName)
        {
            IConfiguration config = new Configuration();
            TelemetryClient = new TelemetryClient();
            this.AppInsightInstrumentationKey = config.GetAppSettingEntry<string>(Constants.AppSettingsKeys.Logging.AppInsightInstrumentationKey);
            TelemetryClient.InstrumentationKey = this.AppInsightInstrumentationKey;

        }
        public AppInsightLogger(IConfiguration config) : base(config)
        {
            //After base create telematry object.
            TelemetryClient = new TelemetryClient();
            this.AppInsightInstrumentationKey = config.GetAppSettingEntry<string>(Constants.AppSettingsKeys.Logging.AppInsightInstrumentationKey);
            TelemetryClient.InstrumentationKey = this.AppInsightInstrumentationKey;
        }

        public AppInsightLogger(string instrumentationKey, string logError, string logInfo, string logWarning, string logTrack)
            : base(logError, logInfo, logWarning, logTrack)
        {
            TelemetryClient = new TelemetryClient();
            if (string.IsNullOrEmpty(instrumentationKey) == false)
            {
                this.AppInsightInstrumentationKey = instrumentationKey;
                TelemetryClient.InstrumentationKey = instrumentationKey;
            }
        }

        #endregion

        #region Properties
        public string AppInsightInstrumentationKey { get; private set; } = string.Empty;
        #endregion

        #region Public Methods

        public void LogError(Exception exception)
        {
            try
            {
                if (IsLogError)
                {
                    Dictionary<string, string> exceptionDictionary = new Dictionary<string, string>();
                    exceptionDictionary.Add("PreciseTimeStamp", DateTime.Now.Ticks.ToString());
                    exceptionDictionary.Add("Error Message", exception.Message);
                    foreach (DictionaryEntry exceptionData in exception.Data)
                    {
                        exceptionDictionary.Add(exceptionData.Key.ToString(), exceptionData.Value.ToString());
                    }
                    exceptionDictionary.Add("LogFileName", logFileName);
                    TelemetryClient.Context.Operation.Id = CorrelationId.ToString();
                    TelemetryClient.TrackException(exception, exceptionDictionary);
                    TelemetryClient.Flush();
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

        public void LogError(string message)
        {
            try
            {
                if (IsLogError)
                {
                    TelemetryClient.Context.Operation.Id = CorrelationId.ToString();
                    ExceptionTelemetry exceptionTelementry = new ExceptionTelemetry();
                    exceptionTelementry.Exception = new Exception("LogFileName- " + logFileName + ":" + message);
                    exceptionTelementry.Properties.Add("PreciseTimeStamp", DateTime.Now.Ticks.ToString());
                    TelemetryClient.TrackException(exceptionTelementry);
                    TelemetryClient.Flush();
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

        public void LogInfo(string message)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("PreciseTimeStamp", DateTime.Now.Ticks.ToString());
            LogInfo(message, properties);
        }

        public void LogInfo(string message, Dictionary<string, string> properties)
        {
            try
            {
                if (IsLogInfo)
                {
                    TelemetryClient.Context.Operation.Id = CorrelationId.ToString();
                    TelemetryClient.TrackTrace("LogFileName- " + logFileName + ":" + message, SeverityLevel.Information, properties);
                    TelemetryClient.Flush();
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

        public void LogWarning(string message)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("PreciseTimeStamp", DateTime.Now.Ticks.ToString());
            LogWarning(message, properties);
        }

        public void LogWarning(string message, Dictionary<string, string> properties)
        {
            try
            {
                if (IsLogWarn)
                {
                    TelemetryClient.Context.Operation.Id = CorrelationId.ToString();
                    TelemetryClient.TrackTrace("LogFileName- " + logFileName + ":" + message, SeverityLevel.Warning, properties);
                    TelemetryClient.Flush();
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

        public void TrackEvent(Enums.EnrollmentEvent eventName, Dictionary<string, string> properties)
        {
            try
            {
                if (IsLogTrack)
                {
                    if (properties != null)
                    {
                        properties.Add("LogFileName", logFileName);
                        properties.Add("PreciseTimeStamp", DateTime.Now.Ticks.ToString());
                    }
                    TelemetryClient.Context.Operation.Id = CorrelationId.ToString();
                    if (properties != null)
                        TelemetryClient.TrackEvent(eventName.ToString(), properties);
                    else
                        TelemetryClient.TrackEvent(eventName.ToString());
                    TelemetryClient.Flush();
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

        public void TrackApplicationEvent(Guid correlationId, Enums.ApplicationEvent eventName, Dictionary<string, string> properties)
        {
            try
            {
                if (IsLogTrack)
                {
                    if (properties != null)
                    {
                        properties.Add("LogFileName", logFileName);
                        properties.Add("PreciseTimeStamp", DateTime.Now.Ticks.ToString());
                    }
                    TelemetryClient.Context.Operation.Id = correlationId.ToString();
                    if (properties != null)
                        TelemetryClient.TrackEvent(eventName.ToString(), properties);
                    else
                        TelemetryClient.TrackEvent(eventName.ToString());
                    TelemetryClient.Flush();
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
                    if (properties != null)
                    {
                        properties.Add("LogFileName", logFileName);
                        properties.Add("PreciseTimeStamp", DateTime.Now.Ticks.ToString());
                    }
                    TelemetryClient.Context.Operation.Id = correlationId.ToString();
                    if (properties != null)
                        TelemetryClient.TrackEvent(eventName.ToString(), properties);
                    else
                        TelemetryClient.TrackEvent(eventName.ToString());
                    TelemetryClient.Flush();
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

        public void LogMethodStart(params string[] parameters)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                MethodBase method = stackTrace.GetFrame(1).GetMethod();
                string methodName = method.Name;
                if (method.ReflectedType != null)
                {
                    string className = method.ReflectedType.Name;
                    StringBuilder inputParameters = new StringBuilder();
                    if (parameters != null && parameters.Length > 0)
                    {
                        for (int x = 0; x < parameters.Length; x++)
                        {
                            inputParameters.Append(parameters[x]);
                            inputParameters.Append(",");
                        }
                    }
                    string formattedMessage = String.Format("{0} : Start Of Method. Parameters {1}", className + ":" + methodName, inputParameters);
                    LogInfo(formattedMessage);
                }
            }
            catch (Exception ex)
            {
                throw new LogMangerException(nameof(ErrorCodeMessages.LoggerError106) + ": " + ErrorCodeMessages.LoggerError106, ex);
            }
        }

        public void LogMethodEnd(params string[] parameters)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                MethodBase method = stackTrace.GetFrame(1).GetMethod();
                string methodName = method.Name;
                if (method.ReflectedType != null)
                {
                    string className = method.ReflectedType.Name;
                    StringBuilder inputParameters = new StringBuilder();
                    if (parameters != null && parameters.Length > 0)
                    {
                        for (int x = 0; x < parameters.Length; x++)
                        {
                            inputParameters.Append(parameters[x]);
                            inputParameters.Append(",");
                        }
                    }
                    string formattedMessage = String.Format("{0} : End Of Method. Parameters {1}", className + ":" + methodName, inputParameters);
                    LogInfo(formattedMessage);
                }
            }
            catch (Exception ex)
            {
                throw new LogMangerException(nameof(ErrorCodeMessages.LoggerError106) + ": " + ErrorCodeMessages.LoggerError106, ex);
            }
        }

        /// <summary>
        /// Method to log details related to request such as time taken for method execution along
        /// with user id and corretion id. Use this method only in OnActionExecuted to capture
        /// desired details
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        public void LogRequestDetails(string userId, DateTime startTime, DateTime endTime, string controllerName, string methodName)
        {
            try
            {
                var executionTime = endTime - startTime;
                string formattedMessage = String.Format("LogFileName - {0}, UserId - {1}. Correlation Id - {2}. Controller Name - {3}. Method Name - {4}. Method call started at - {5}. Method call end at - {6}. Method execution time - {7} ", logFileName, string.IsNullOrEmpty(userId) ? "Not Available" : userId, CorrelationId, controllerName, methodName, startTime.ToString("HH:mm:ss.fff"), endTime.ToString("HH:mm:ss.fff"), executionTime);
                TelemetryClient.Context.Operation.Id = CorrelationId.ToString();
                TelemetryClient.TrackTrace(formattedMessage, SeverityLevel.Information);
                TelemetryClient.Flush();
            }
            catch (Exception ex)
            {
                throw new LogMangerException(nameof(ErrorCodeMessages.LoggerError106) + ": " + ErrorCodeMessages.LoggerError106, ex);
            }
        }

        //below is not in use.
        public string GetDetailsfromLogger(DateTime startDatetime, DateTime endDateTime)
        {
            try
            {
                var timeSpan = endDateTime - startDatetime;
                var client = new HttpClient();
                var returnValues = new List<LogMessage>();

                var url = "https://api.applicationinsights.io/v1/apps/76dcb455-6c52-49f2-bf84-d97008e3cf7b/query?timespan=" + startDatetime + "%2" + endDateTime + "&query=requests%7C%20where%20timestamp%20%3E%3D%20ago(24h)";

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-api-key", System.Configuration.ConfigurationManager.AppSettings["apiKey"]);

                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    return response.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                throw new LogMangerException(nameof(ErrorCodeMessages.LoggerError106) + ": " + ErrorCodeMessages.LoggerError106, ex);
            }
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

        #endregion Public Methods

    }
}