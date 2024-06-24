using Core.Log;
using System;

namespace Core.Helper
{
    public static class LoggingHelper
    {
        public static bool IsConsole = false;
        
        public static void LogMessage(string message)
        {
            // Beispiel: Hier könnte die Implementierung für das Logging stehen
            if(IsConsole)
                Console.WriteLine($"[INFO] {DateTime.Now}: {message}");
            else { LoggingManager.LogMessage(message); }
        }

        public static void LogError(string errorMessage, string functionName, int lineNumber)
        {
            // Beispiel: Hier könnte die Implementierung für das Logging stehen
            if (IsConsole)
                Console.WriteLine($"[ERROR] {DateTime.Now}: {errorMessage} (Function: {functionName}, Line: {lineNumber})");
            else { LoggingManager.LogError(errorMessage, functionName, lineNumber); }
        }
    }
}
