using System;

namespace Core.Interfaces
{
    public interface ITestBase
    {
        void Run(Action action);
        void RunAction(Action action);
        void Start();
        void Stop();
    }
}