using System;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Log;

namespace Core
{
    public class ConfigObserver : IConfigObserver
    {
        private readonly Dictionary<string, string> configSettings;

        public ConfigObserver()
        {
            configSettings = new Dictionary<string, string>
            {
                { "LogLevel", "Info" },
                { "MaxConnections", "10" },
                { "TimeoutInSeconds", "30" }
            };
        }

        public void UpdateConfig(string key, string value)
        {
            if (configSettings.ContainsKey(key))
            {
                configSettings[key] = value;
                LoggingManager.LogMessage($"Config setting '{key}' updated to '{value}'");
            }
            else
            {
                LoggingManager.LogMessage($"Config setting '{key}' does not exist.");
            }
        }

        public string GetConfigValue(string key)
        {
            if (configSettings.ContainsKey(key))
            {
                return configSettings[key];
            }
            return string.Empty;
        }

        public void AddOrUpdateConfig(string key, string value)
        {
            if (configSettings.ContainsKey(key))
            {
                configSettings[key] = value;
            }
            else
            {
                configSettings.Add(key, value);
            }
        }
    }
}