using rsoni.LogManager.ExceptionTypes;
using rsoni.LogManager.Common;
using System;
using System.Collections.Generic;

namespace rsoni.LogManager
{
    public interface ILogger
    {
        Guid CorrelationId { get; set; }

        #region Methods

        void LogEntry(Enums.LogType logType, string message);

        void LogEntry(Enums.LogType logType, string message, Exception exception);

        void LogError(string message);

        void LogError(Exception exception);

        void LogInfo(string message);

        void LogWarning(string message);

        void TrackEvent(Enums.EnrollmentEvent eventName, Dictionary<string, string> properties = null);

        void TrackApplicationEvent(Guid correlationId, Enums.ApplicationEvent eventName, Dictionary<string, string> properties);

        void TrackSystemEvent(Guid correlationId, Enums.EnrollmentEvent eventName, Dictionary<string, string> properties);

        // ReSharper disable once UnusedParameter.Global
        void LogMethodStart(params string[] parameters);

        // ReSharper disable once UnusedParameter.Global
        void LogMethodEnd(params string[] parameters);

        void LogRequestDetails(string userId, DateTime startTime, DateTime endTime, string controllerName, string methodName);

        string GetDetailsfromLogger(DateTime startDatetime, DateTime endDateTime);

        #endregion Methods
    }
}