using rsoni.LogManager.Standard.Common;
using System;

namespace rsoni.LogManager.Standard 
{
    public class ApplicationEventDetails
    {
        public Guid CorrelationId { get; set; }
        public Enums.ApplicationEvent ApplicationEvent { get; set; }
        public string Username { get; set; }
        public DateTime EventTriggerDateTime { get; set; }
    }
}