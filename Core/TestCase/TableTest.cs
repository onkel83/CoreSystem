using Core.Config;
using Core.ConsoleHelper;
using Core.Logging;
using Core.Models;
using System;
using System.Collections.Generic;

namespace Core.TestCase
{
    public class TableTest : TestBase
    {
        //private Logger _log = Logger.Instance;
        //private ConfigLoader _conf = ConfigLoader.Instance;

        private static TableTest _Instance = new TableTest();
        public static TableTest Instance { get => _Instance ?? new TableTest(); private set => _Instance = value; }

        private TableTest()
        {
        }

        public override void Start()
        {
            Console.WriteLine("Start des Tabletests");
        }

        public override void RunAction(Action action)
        {
            action();
        }

        public override void Stop()
        {
            Console.WriteLine("Ende des Tabletests, bitte drücken Sie [Enter]");
            Console.ReadLine();
        }

        public void RunTableTests()
        {
            Console.WriteLine("Core Project started...");

            var items = new List<TestModel>
            {
                new TestModel { ID = 1, Name = "Test Item 1" },
                new TestModel { ID = 2, Name = "Test Item 2 with a very long name that should wrap" },
                new TestModel { ID = 3, Name = "Test Item 3" }
            };

            TableRenderer<TestModel>.Render(items);
        }
    }
}
