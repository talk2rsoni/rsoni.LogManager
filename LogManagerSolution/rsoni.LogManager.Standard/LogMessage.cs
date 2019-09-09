using rsoni.LogManager.Standard.ExceptionTypes;
using rsoni.LogManager.Standard.Common;
using Microsoft.ApplicationInsights.DataContracts;
using System;

namespace rsoni.LogManager.Standard
{
    public class LogMessage
    {
        public Guid CorrelationId { get; set; }

        public string Message { get; set; } = string.Empty;

        public Enums.LogType LogType { get; set; }

        public Exception Exception { get; set; }

        public SeverityLevel SeverityLevel { get; set; }

        public DateTime EventDateTime { get; set; }
    }
}