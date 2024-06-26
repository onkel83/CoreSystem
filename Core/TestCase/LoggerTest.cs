﻿using Core.Config;
using Core.Interfaces;
using Core.Logging;
using System;

namespace Core.TestCase
{
    public class LoggerTest : TestBase
    {
        private Logger _log = Logger.Instance;

        private static LoggerTest _Instance = new LoggerTest();
        public static LoggerTest Instance { get => _Instance ?? new LoggerTest(); private set => _Instance = value; }

        private LoggerTest()
        {
        }

        public override void Start()
        {
            Console.WriteLine("Start des Logger Tests");
        }

        public override void RunAction(Action action)
        {
            action();
        }

        public override void Stop()
        {
            Console.WriteLine("Ende des Logger Tests, bitte drücken Sie [Enter]");
            Console.ReadLine();
        }

        public void RunLoggerTests()
        {
            _log.RegisterObserver(new ConsoleLogObserver());

            _log.Log(LogLevel.Info, "Info log message");
            _log.Log(LogLevel.Warn, "Warning log message");
            _log.Log(LogLevel.Err, "Error log message");

            _log.UnregisterObserver(new ConsoleLogObserver());
        }
    }
}
