using System;
using System.IO;

namespace Core.Log
{
    public static class LoggingManager
    {
        private static readonly ConsoleLoggingTarget cLT;
        private static readonly FileLoggingTarget cFT;
        public static bool IsConsole { get; set; } = true;
        public static bool IsFile { get; set; } = true;

        private static readonly string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        public static string FileName { get; set; } = string.Empty;

        static LoggingManager()
        {
            IsConsole = false;
            IsFile = true;
            cLT = new ConsoleLoggingTarget();
            cFT = new FileLoggingTarget((string.IsNullOrEmpty(FileName)) ? "log.txt" : FileName);
            EnsureLogFolderExists();
        }

        private static void EnsureLogFolderExists()
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        public static void LogError(string errorMessage, string functionName, int lineNumber)
        {
            if (IsConsole)
            {
                cLT.LogError(errorMessage, functionName, lineNumber);
            }
            if (IsFile)
            {
                cFT.LogError(errorMessage, functionName, lineNumber);
            }
        }

        public static void LogMessage(string message)
        {
            if (IsConsole)
            {
                cLT.LogMessage(message);
            }
            if (IsFile)
            {
                cFT.LogMessage(message);
            }
        }
    }
}