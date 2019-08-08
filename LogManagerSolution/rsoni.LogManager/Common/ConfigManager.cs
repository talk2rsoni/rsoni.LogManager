using rsoni.LogManager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rsoni.Common
{
    public static class ConfigManager
    {
        static IConfiguration config;
        static ConfigManager()
        {
            config = new Configuration();

        }

        public static string GetConfigEntry(string appSettingKey)
        {
            string returnValue = string.Empty;
            returnValue = config.GetAppSettingEntry<string>(appSettingKey);

            return returnValue;

        }
    }
}
