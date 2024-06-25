using System.Threading.Tasks;
namespace Core.Interfaces
{
    public enum LogLevel
    {
        Crit,
        Warn,
        Err,
        Message,
        Info,
        Debug
    }

    public interface ILogObserver
    {
        Task OnLogMessageAsync(string message);
    }
}