using rsoni.LogManager.Common;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;

namespace rsoni.LogManager
{
    public class BaseLogger : ILogger
    {
        public bool IsLogError;
        public bool IsLogInfo;
        public bool IsLogWarn;
        public bool IsLogTrack;

        internal readonly TelemetryClient TelemetryClient;
        private IConfiguration Configuration;

        public Guid CorrelationId { get; set; } = Guid.NewGuid();

        public BaseLogger(IConfiguration configuration)
        {
            Configuration = configuration;
            IsLogError = configuration.GetAppSettingEntry<Boolean>(Constants.AppSettingsKeys.Logging.LogError);
            IsLogInfo = configuration.GetAppSettingEntry<Boolean>(Constants.AppSettingsKeys.Logging.LogInfo);
            IsLogWarn = configuration.GetAppSettingEntry<Boolean>(Constants.AppSettingsKeys.Logging.LogWarning);
            IsLogTrack = configuration.GetAppSettingEntry<Boolean>(Constants.AppSettingsKeys.Logging.LogTrack);

            TelemetryClient = new TelemetryClient();
            TelemetryClient.InstrumentationKey = configuration.GetAppSettingEntry<string>(Constants.AppSettingsKeys.Logging.AppInsightInstrumentationKey);
        }

        #region Implement Interface.

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

        public virtual void LogError(string message)
        {
            throw new NotImplementedException();
        }

        public virtual void LogError(Exception exception)
        {
            throw new NotImplementedException();
        }

        public virtual void LogInfo(string message)
        {
            throw new NotImplementedException();
        }

        public virtual void LogWarning(string message)
        {
            throw new NotImplementedException();
        }

        public virtual void TrackEvent(Enums.EnrollmentEvent eventName, Dictionary<string, string> properties = null)
        {
            throw new NotImplementedException();
        }

        public virtual void TrackApplicationEvent(Guid correlationId, Enums.ApplicationEvent eventName, Dictionary<string, string> properties)
        {
            throw new NotImplementedException();
        }

        public virtual void TrackSystemEvent(Guid correlationId, Enums.EnrollmentEvent eventName, Dictionary<string, string> properties)
        {
            throw new NotImplementedException();
        }

        public virtual void LogMethodStart(params string[] parameters)
        {
            throw new NotImplementedException();
        }

        public virtual void LogMethodEnd(params string[] parameters)
        {
            throw new NotImplementedException();
        }

        public virtual void LogRequestDetails(string userId, DateTime startTime, DateTime endTime, string controllerName, string methodName)
        {
            throw new NotImplementedException();
        }

        public virtual string GetDetailsfromLogger(DateTime startDatetime, DateTime endDateTime)
        {
            throw new NotImplementedException();
        }

        #endregion Implement Interface.
    }
}