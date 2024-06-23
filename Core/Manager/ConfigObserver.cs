using System;
using System.Collections.Generic;
using Core.Interfaces;

namespace Core
{
    public class ConfigObserver : IConfigObserver
    {
        private readonly Dictionary<string, string> configSettings;

        public ConfigObserver()
        {
            // Beispiel: Initialisierung der Konfigurationseinstellungen
            configSettings = new Dictionary<string, string>
            {
                { "LogLevel", "Info" },
                { "MaxConnections", "10" },
                { "TimeoutInSeconds", "30" }
                // Weitere Konfigurationseinstellungen können hier hinzugefügt werden
            };
        }

        public void UpdateConfig(string key, string value)
        {
            if (configSettings.ContainsKey(key))
            {
                configSettings[key] = value;
                Console.WriteLine($"Config setting '{key}' updated to '{value}'");
                // Hier könnten weitere Aktionen abhängig von der aktualisierten Konfiguration erfolgen
            }
            else
            {
                Console.WriteLine($"Config setting '{key}' does not exist.");
            }
        }

        // Beispielmethoden zum Abrufen von Konfigurationseinstellungen
        public string GetConfigValue(string key)
        {
            if (configSettings.ContainsKey(key))
            {
                return configSettings[key];
            }
            return string.Empty; // Oder einen Standardwert zurückgeben, je nach Anwendungslogik
        }

        // Beispielmethoden zum Hinzufügen oder Aktualisieren von Konfigurationseinstellungen
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
