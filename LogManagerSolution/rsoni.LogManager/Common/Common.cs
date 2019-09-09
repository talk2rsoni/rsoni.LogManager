using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsoni.LogManager.Common
{
    internal static class Constants
    {
        #region AppSetting keys
        internal static AppSettingsContants AppSettingsKeys = new AppSettingsContants();
        #endregion


        public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }

    }
    internal class AppSettingsContants
    {
        internal LoggingConstants Logging = new LoggingConstants();

    }

    internal class LoggingConstants
    {

        public string AppInsightInstrumentationKey
        {
            get
            {
                return "AppInsight-InstrumentationKey";
            }
        }

        public string LogError
        {
            get
            {
                return "LogError";
            }
        }

        public string LogInfo
        {
            get
            {
                return "LogInfo";
            }
        }


        public string LogWarning
        {
            get
            {
                return "LogWarning";
            }
        }


        public string LogTrack
        {
            get
            {
                return "LogTrack";
            }
        }
    }


    public class Enums
    {

        public enum LogType
        {
            None,
            Error,
            Warning,
            Information,
            Track
        }

        public enum ApplicationEvent
        {
            None,
            ProductGet,
        }

        public enum EnrollmentEvent
        {
            None,
            AzureSQLDatabaseCalled,
            AzureStorageCalled,
            AzureRedisCacheCalled,
            AzureKeyVaultCalled,
            AzureServiceBusCalled
        }
    }


    public interface IConfiguration
    {
        TType GetAppSettingEntry<TType>(string AppSettingKey);
    }

    internal class Configuration : IConfiguration
    {

        /// <summary>
        /// Get the config Entry
        /// </summary>
        /// <param name="AppSettingKey"></param>
        /// <returns></returns>
        public TType GetAppSettingEntry<TType>(string AppSettingKey)
        {
            string configEntryString = string.Empty;
            try
            {
                configEntryString = ConfigurationManager.AppSettings[AppSettingKey];
                if (string.IsNullOrEmpty(configEntryString))
                {
                    configEntryString = "";

                    if (typeof(TType) == typeof(int))
                    {
                        configEntryString = "0";
                    }
                    if (typeof(TType) == typeof(bool) || typeof(TType) == typeof(Boolean))
                    {
                        configEntryString = "false";
                    }

                }
                else
                {
                    configEntryString = Convert.ToString(ConfigurationManager.AppSettings[AppSettingKey]);
                }
            }
            catch (Exception ex)
            {
                // No catch for exception.
            }


            return (TType)Convert.ChangeType(configEntryString, typeof(TType));
        }
    }        
}
