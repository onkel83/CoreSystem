using System;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Logging
{
    public class ConsoleLogObserver : ILogObserver
    {
        public Task OnLogMessageAsync(string message)
        {
            Console.WriteLine("[Beobachter] " + message);
            return Task.CompletedTask;
        }
    }
}
