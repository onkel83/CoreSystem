using System;
using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Manager
{
    public static class ConfigManager
    {
        private static readonly Dictionary<string, string> configValues = new Dictionary<string, string>();

        private static readonly List<IConfigObserver> observers = new List<IConfigObserver>();

        /// <summary>
        /// Fügt einen Beobachter für Konfigurationsänderungen hinzu.
        /// </summary>
        /// <param name="observer">Der Beobachter, der hinzugefügt werden soll.</param>
        public static void AddObserver(IConfigObserver observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        /// <summary>
        /// Entfernt einen Beobachter für Konfigurationsänderungen.
        /// </summary>
        /// <param name="observer">Der Beobachter, der entfernt werden soll.</param>
        public static void RemoveObserver(IConfigObserver observer)
        {
            observers.Remove(observer);
        }

        /// <summary>
        /// Aktualisiert den Wert einer Konfigurationseinstellung.
        /// </summary>
        /// <param name="key">Der Schlüssel der Konfigurationseinstellung.</param>
        /// <param name="value">Der neue Wert der Konfigurationseinstellung.</param>
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

        /// <summary>
        /// Ruft den aktuellen Wert einer Konfigurationseinstellung ab.
        /// </summary>
        /// <param name="key">Der Schlüssel der Konfigurationseinstellung.</param>
        /// <returns>Der aktuelle Wert der Konfigurationseinstellung.</returns>
        public static string GetConfigValue(string key)
        {
            return configValues.ContainsKey(key) ? configValues[key] : string.Empty;
        }

        /// <summary>
        /// Benachrichtigt alle Beobachter über eine Konfigurationsänderung.
        /// </summary>
        /// <param name="key">Der Schlüssel der geänderten Konfigurationseinstellung.</param>
        /// <param name="value">Der neue Wert der Konfigurationseinstellung.</param>
        private static void NotifyObservers(string key, string value)
        {
            foreach (var observer in observers)
            {
                observer.UpdateConfig(key, value);
            }
        }
    }
}
