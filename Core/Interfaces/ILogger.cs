using Core.Interfaces;

namespace Core.Interfaces
{
    public interface ILogger
    {
        void Log(LogLevel level, string message, string caller = "");
        void RegisterObserver(ILogObserver observer);
        void UnregisterObserver(ILogObserver observer);
    }
}
