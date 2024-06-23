using System;

namespace Core.Log
{
    public class ConsoleLoggingTarget
    {
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
            Console.WriteLine(logMessage);
        }
    }
}
