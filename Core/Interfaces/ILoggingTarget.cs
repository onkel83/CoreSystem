namespace Core.Interfaces
{
    public interface ILoggingTarget
    {
        void LogMessage(string message);

        void LogError(string errorMessage, string functionName, int lineNumber);
    }
}
