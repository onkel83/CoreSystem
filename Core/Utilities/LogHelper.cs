using Core.Logging;
using Core.Interfaces;

namespace Core.Utilities
{
    public static class LogHelper
    {
        private readonly static ILogger _logger = Logger.Instance;

        public static void Log(LogLevel level, string message, string caller = "")
        {
            _logger.Log(level, message, caller);
        }

        public static void RegisterObserver(ILogObserver observer)
        {
            _logger.RegisterObserver(observer);
        }

        public static void UnregisterObserver(ILogObserver observer)
        {
            _logger.UnregisterObserver(observer);
        }
    }
}
