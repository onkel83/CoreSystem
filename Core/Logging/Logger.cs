using Core.Config;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Logging
{
    public sealed class Logger : ILogger
    {
        private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());
        private readonly List<ILogObserver> _observers = new List<ILogObserver>();
        private readonly object _lock = new object();
        private readonly Timer _timer;
        private string _logFilePath = string.Empty;
        private string _messageFilePath = string.Empty;
        private string _errorFilePath = string.Empty;
        private bool _logToFile = true;
        private bool _logToConsole = true;

        private Logger()
        {
            LoadConfig();
            EnsureDirectoryExists(_logFilePath);
            _timer = new Timer(RotateLogs, null, TimeSpan.Zero, TimeSpan.FromDays(30));
            _ = _timer;
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public static Logger Instance => _instance.Value;

        public void RegisterObserver(ILogObserver observer)
        {
            lock (_lock)
            {
                _observers.Add(observer);
            }
        }

        public void UnregisterObserver(ILogObserver observer)
        {
            lock (_lock)
            {
                _observers.Remove(observer);
            }
        }

        public void Log(LogLevel level, string message, string caller = "")
        {
            string logMessage = FormatLogMessage(level, message, caller);
            WriteLog(level, logMessage);
            NotifyObservers(logMessage);
        }

        private void WriteLog(LogLevel level, string message)
        {
            lock (_lock)
            {
                try
                {
                    if (_logToFile)
                    {
                        string filePath = GetFilePath(level);
                        if (filePath != null)
                        {
                            File.AppendAllText(filePath, message + Environment.NewLine);
                        }
                    }

                    if (_logToConsole)
                    {
                        Console.WriteLine(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to write log: {ex.Message}");
                }
            }
        }

        private string GetFilePath(LogLevel level)
        {
            return level == LogLevel.Message || level == LogLevel.Info ? _messageFilePath : _errorFilePath;
        }

        private string FormatLogMessage(LogLevel level, string message, string caller)
        {
            string timeStamp = DateTime.Now.ToString("HH:mm dd.MM.yyyy");
            return level switch
            {
                LogLevel.Crit => $"[{timeStamp}]:[{level}]:[{message}]:[{caller}]",
                LogLevel.Warn => $"[{timeStamp}]:[{level}]:[{message}]:[{caller}]",
                LogLevel.Err => $"[{timeStamp}]:[{level}]:[{message}]:[{caller}]",
                LogLevel.Message => $"{message}",
                LogLevel.Info => $"[{timeStamp}]:[{message}]",
                LogLevel.Debug => $"[{timeStamp}]:[{message}][{caller}]",
                _ => message
            };
        }

        private async void NotifyObservers(string message)
        {
            List<Task> tasks;
            lock (_lock)
            {
                tasks = _observers.Select(o => o.OnLogMessageAsync(message)).ToList();
            }
            await Task.WhenAll(tasks);
        }

        private void RotateLogs(object state)
        {
            lock (_lock)
            {
                RotateLog(_messageFilePath);
                RotateLog(_errorFilePath);
            }
        }

        private void RotateLog(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string backupPath = filePath + ".bak";
                    if (File.Exists(backupPath))
                    {
                        File.Delete(backupPath);
                    }
                    File.Move(filePath, backupPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to rotate log: {ex.Message}");
            }
        }

        private void LoadConfig()
        {
            try
            {
                ConfigLoader.Instance.LoadConfig("LoggerConf.json");
                string logFilePath = ConfigLoader.Instance.GetValue("LogFilePath", "LoggerConf.json");
                string logToFile = ConfigLoader.Instance.GetValue("LogToFile", "LoggerConf.json");
                string logToConsole = ConfigLoader.Instance.GetValue("LogToConsole", "LoggerConf.json");

                _logFilePath = !string.IsNullOrWhiteSpace(logFilePath) ? logFilePath : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                _messageFilePath = Path.Combine(_logFilePath, "messages.log");
                _errorFilePath = Path.Combine(_logFilePath, "errors.log");
                _logToFile = !string.IsNullOrEmpty(logToFile) && bool.Parse(logToFile);
                _logToConsole = !string.IsNullOrEmpty(logToConsole) && bool.Parse(logToConsole);

                EnsureDirectoryExists(_logFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load logger config: {ex.Message}");
            }
        }
    }
}