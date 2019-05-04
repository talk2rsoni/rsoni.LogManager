
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

    public class AppInsightLogger : BaseLogger
    {
        #region Fields
        private string logFileName;

        #endregion Fields

        public AppInsightLogger(string logName) : base(new Configuration())
        {
            logFileName = logName;
        }

        #region Methods

        #region Public Methods

        public override void LogError(Exception exception)
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

        public override void LogError(string message)
        {
            try
            {
                if (IsLogError)
                {
                    TelemetryClient.Context.Operation.Id = CorrelationId.ToString();
                    ExceptionTelemetry exceptionTelementry = new ExceptionTelemetry();
                    exceptionTelementry.Exception = new Exception("LogFileName- " + logFileName + ":" + message);
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

        public override void LogInfo(string message)
        {
            try
            {
                if (IsLogInfo)
                {
                    TelemetryClient.Context.Operation.Id = CorrelationId.ToString();
                    TelemetryClient.TrackTrace("LogFileName- " + logFileName + ":" + message, SeverityLevel.Information);
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

        public override void LogWarning(string message)
        {
            try
            {
                if (IsLogWarn)
                {
                    TelemetryClient.Context.Operation.Id = CorrelationId.ToString();
                    TelemetryClient.TrackTrace("LogFileName- " + logFileName + ":" + message, SeverityLevel.Warning);
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

        public override void TrackEvent(Enums.EnrollmentEvent eventName, Dictionary<string, string> properties)
        {
            try
            {
                if (IsLogTrack)
                {
                    if (properties != null)
                        properties.Add("LogFileName", logFileName);
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

        public override void TrackApplicationEvent(Guid correlationId, Enums.ApplicationEvent eventName, Dictionary<string, string> properties)
        {
            try
            {
                if (IsLogTrack)
                {
                    if (properties != null)
                        properties.Add("LogFileName", logFileName);
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

        public override void TrackSystemEvent(Guid correlationId, Enums.EnrollmentEvent eventName, Dictionary<string, string> properties)
        {
            try
            {
                if (IsLogTrack)
                {
                    if (properties != null)
                        properties.Add("LogFileName", logFileName);
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

        public override void LogMethodStart(params string[] parameters)
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

        public override void LogMethodEnd(params string[] parameters)
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
        public override void LogRequestDetails(string userId, DateTime startTime, DateTime endTime, string controllerName, string methodName)
        {
            try
            {
                var executionTime = endTime - startTime;
                string formattedMessage = String.Format("LogFileName - {0}, UserId - {1}. Correlation Id - {2}. Controller Name - {3}. Method Name - {4}. Method call started at - {5}. Method call end at - {6}. Method execution time - {7} ", logFileName, string.IsNullOrEmpty(userId) ? "Not Available" : userId, CorrelationId, controllerName, methodName, startTime, endTime, executionTime);
                TelemetryClient.Context.Operation.Id = CorrelationId.ToString();
                TelemetryClient.TrackTrace(formattedMessage, SeverityLevel.Information);
                TelemetryClient.Flush();
            }
            catch (Exception ex)
            {
                throw new LogMangerException(nameof(ErrorCodeMessages.LoggerError106) + ": " + ErrorCodeMessages.LoggerError106, ex);
            }
        }

        public override string GetDetailsfromLogger(DateTime startDatetime, DateTime endDateTime)
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

        #endregion Public Methods

        #endregion Methods
    }
}