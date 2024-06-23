using System;
using System.IO;

namespace Core.Log
{
    public class FileLoggingTarget
    {
        private readonly string logFilePath;
        private readonly string logFileName;
        private readonly string fullPath;

        public FileLoggingTarget(string fileName)
        {
            logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            logFileName = fileName;
            if (!Directory.Exists(logFilePath))
            {
                Directory.CreateDirectory(logFilePath);
            }

            fullPath = Path.Combine(logFilePath, logFileName);
        }

        public void LogMessage(string message)
        {
            Log($"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        public void LogError(string errorMessage, string functionName, int lineNumber)
        {
            Log($"[{DateTime.Now:HH:mm:ss}] ERROR: {errorMessage} in {functionName}, line {lineNumber}");
        }

        private void Log(string logMessage)
        {
            try
            {
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }
                using StreamWriter writer = new StreamWriter(fullPath, true);
                writer.WriteLine(logMessage);
                writer.Flush();
                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging to file: {ex.Message}");
            }
        }
    }
}
