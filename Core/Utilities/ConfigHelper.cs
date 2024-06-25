using System.Collections.Generic;
using Core.Config;

namespace Core.Utilities
{
    public static class ConfigHelper
    {
        private readonly static ConfigLoader _configLoader = ConfigLoader.Instance;

        public static string GetConfigValue(string key, string configFileName)
        {
            return _configLoader.GetValue(key, configFileName);
        }

        public static void SetConfigValue(string configFileName, string key, string value)
        {
            _configLoader.Update(configFileName, key, value);
        }

        public static List<string> ListConfigFiles()
        {
            return _configLoader.ListConfigFiles();
        }
    }
}
