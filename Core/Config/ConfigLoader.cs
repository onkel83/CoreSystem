using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading;
using Core.Interfaces;
using System.Linq;

namespace Core.Config
{
    public sealed class ConfigLoader : IConfigLoader
    {
        private static readonly Lazy<ConfigLoader> _instance = new Lazy<ConfigLoader>(() => new ConfigLoader());
        private Dictionary<string, Dictionary<string, string>> _configs = new Dictionary<string, Dictionary<string, string>>();
        private readonly Dictionary<string, Dictionary<string, string>> _defaultConfig;
        private readonly Timer _timer;
        private readonly object _lock = new object();

        private ConfigLoader()
        {
            _timer = new Timer(CheckForChanges, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }

            _defaultConfig = new Dictionary<string, Dictionary<string, string>>(DefaultConfig.GetDefault());
            LoadAllConfigs();
            SaveAllDefaultConfigs();
            StartMonitoring();
        }

        public static ConfigLoader Instance => _instance.Value;

        public string FilePath { get; set; }
        public Dictionary<string, Dictionary<string, string>> Config { get => _configs; private set => _configs = value; }

        public void LoadConfig(string configFileName)
        {
            if (_configs.ContainsKey(configFileName))
            {
                _configs[configFileName].Clear();
            }
            else
            {
                _configs[configFileName] = new Dictionary<string, string>();
            }

            var filePath = Path.Combine(FilePath, configFileName);
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                if (config != null)
                {
                    _configs[configFileName] = config;
                }
            }
        }

        public void SaveConfig(string configFileName)
        {
            lock (_lock)
            {
                if (_configs.ContainsKey(configFileName))
                {
                    var filePath = Path.Combine(FilePath, configFileName);
                    var json = JsonConvert.SerializeObject(_configs[configFileName], Formatting.Indented);
                    File.WriteAllText(filePath, json);
                }
            }
        }

        public bool Add(string configFileName, string key, string value)
        {
            lock (_lock)
            {
                key = key.ToLower();
                value = value.Trim();

                if (!_configs.ContainsKey(configFileName))
                {
                    _configs[configFileName] = new Dictionary<string, string>();
                }

                _configs[configFileName][key] = value;
                SaveConfig(configFileName);
                return true;
            }
        }

        public bool Delete(string configFileName, string key)
        {
            lock (_lock)
            {
                key = key.ToLower();

                if (_configs.ContainsKey(configFileName) && _configs[configFileName].ContainsKey(key))
                {
                    _configs[configFileName].Remove(key);
                    SaveConfig(configFileName);
                    return true;
                }
            }

            return false;
        }

        public bool Update(string configFileName, string key, string value)
        {
            lock (_lock)
            {
                key = key.ToLower();
                value = value.Trim();

                if (_configs.ContainsKey(configFileName) && _configs[configFileName].ContainsKey(key))
                {
                    _configs[configFileName][key] = value;
                    SaveConfig(configFileName);
                    return true;
                }
            }

            return false;
        }

        public string GetValue(string key, string configFileName)
        {
            key = key.ToLower();

            if (_configs.ContainsKey(configFileName) && _configs[configFileName].ContainsKey(key))
            {
                return _configs[configFileName][key];
            }

            foreach (var defaultConfig in _defaultConfig)
            {
                if (defaultConfig.Value.ContainsKey(key))
                {
                    Add(configFileName, key, defaultConfig.Value[key]);
                    return defaultConfig.Value[key];
                }
            }

            return string.Empty;
        }

        private void LoadAllConfigs()
        {
            var configFiles = Directory.GetFiles(FilePath, "*.json");
            foreach (var configFile in configFiles)
            {
                var configFileName = Path.GetFileName(configFile);
                LoadConfig(configFileName);
            }
        }

        private void SaveAllDefaultConfigs()
        {
            foreach (var kvp in _defaultConfig)
            {
                foreach (var item in kvp.Value)
                {
                    Add(kvp.Key, item.Key, item.Value);
                }
                SaveConfig(kvp.Key);
            }
        }

        private void StartMonitoring()
        {
            _ = _timer;
        }

        private void CheckForChanges(object state)
        {
            lock (_lock)
            {
                LoadAllConfigs();
            }
        }

        public List<string> ListConfigFiles()
        {
            return Directory.GetFiles(FilePath, "*.json").Select(Path.GetFileName).ToList();
        }
    }
}
