using System;
using Core.Interfaces;

namespace Core.TestCase
{
    public abstract class TestBase : ITestBase
    {

        public abstract void Start();

        public void Run(Action action)
        {
            Start();
            RunAction(action);
            Stop();

        }
        public abstract void RunAction(Action action);
        public abstract void Stop();
    }
}