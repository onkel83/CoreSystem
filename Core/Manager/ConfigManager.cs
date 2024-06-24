// Core/Manager/ConfigManager.cs
using System;
using System.Collections.Generic;
using Core.Config;
using Core.Interfaces;

namespace Core.Manager
{
    public static class ConfigManager
    {
        private static readonly Dictionary<string, string> configValues = new Dictionary<string, string>(DefaultConfig.Settings);
        private static readonly List<IConfigObserver> observers = new List<IConfigObserver>();

        public static void AddObserver(IConfigObserver observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        public static void RemoveObserver(IConfigObserver observer)
        {
            observers.Remove(observer);
        }

        public static void UpdateConfig(string key, string value)
        {
            if (configValues.ContainsKey(key))
            {
                configValues[key] = value;
            }
            else
            {
                configValues.Add(key, value);
            }

            NotifyObservers(key, value);
        }

        public static string GetConfigValue(string key)
        {
            return configValues.ContainsKey(key) ? configValues[key] : string.Empty;
        }

        private static void NotifyObservers(string key, string value)
        {
            foreach (var observer in observers)
            {
                observer.UpdateConfig(key, value);
            }
        }
    }
}
