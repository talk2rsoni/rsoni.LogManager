using rsoni.LogManager.Common;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;

namespace rsoni.LogManager
{
    public class BaseLogger
    {
        public bool IsLogError;
        public bool IsLogInfo;
        public bool IsLogWarn;
        public bool IsLogTrack;

        private IConfiguration Configuration;

        public Guid CorrelationId { get; set; } = Guid.NewGuid();

        public string LogFileName { get; set; } = "Logger";

        public BaseLogger(string logName)
        {
            this.LogFileName = logName;
            InitilizeLoggerFromAPPConfig(new Configuration());
        }
        public BaseLogger(IConfiguration configuration)
        {
            InitilizeLoggerFromAPPConfig(configuration);
        }

        public BaseLogger(string logError, string logInfo, string logWarning, string logTrack)
        {
            InitilizeLogger(null, string.Empty, logError, logInfo, logWarning, logTrack);
        }


        #region initilize constructor

        private void InitilizeLoggerFromAPPConfig(IConfiguration configuration)
        {
            Configuration = configuration;
            IsLogError = configuration.GetAppSettingEntry<Boolean>(Constants.AppSettingsKeys.Logging.LogError);
            IsLogInfo = configuration.GetAppSettingEntry<Boolean>(Constants.AppSettingsKeys.Logging.LogInfo);
            IsLogWarn = configuration.GetAppSettingEntry<Boolean>(Constants.AppSettingsKeys.Logging.LogWarning);
            IsLogTrack = configuration.GetAppSettingEntry<Boolean>(Constants.AppSettingsKeys.Logging.LogTrack);
        }
        private void InitilizeLogger(IConfiguration config, string logFileName, string logError, string logInfo, string logWarning, string logTrack)
        {

            if (config == null)
                Configuration = new Configuration();

            if (string.IsNullOrEmpty(logFileName) == false)
                this.LogFileName = logFileName;

            IsLogError = false;
            IsLogInfo = false;
            IsLogWarn = false;
            IsLogTrack = false;

            if (string.IsNullOrEmpty(logError) == false && logError.ToLower() == "true")
            {
                IsLogError = true;
            }

            if (string.IsNullOrEmpty(logInfo) == false && logInfo.ToLower() == "true")
            {
                IsLogInfo = true;
            }

            if (string.IsNullOrEmpty(logWarning) == false && logWarning.ToLower() == "true")
            {
                IsLogWarn = true;
            }

            if (string.IsNullOrEmpty(logTrack) == false && logTrack.ToLower() == "true")
            {
                IsLogTrack = true;
            }

        }

        #endregion
    }
}