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
        private readonly Logger _log = Logger.Instance;
        private readonly ConfigLoader _conf = ConfigLoader.Instance;

        private static TableTest _Instance = new TableTest();
        public static TableTest Instance { get => _Instance?? new TableTest(); private set => _Instance = value; }

        private TableTest()
        {
            Console.WriteLine("Core Project started...");

            var items = new List<TestModel>
            {
                new TestModel { ID = 1, Name = "Test Item 1" },
                new TestModel { ID = 2, Name = "Test Item 2 with a very long name that should wrap" },
                new TestModel { ID = 3, Name = "Test Item 3" }
            };

            base.Run(() => TableRenderer<TestModel>.Render(items));
        }

        public override void Start()
        {
            Console.WriteLine($"Start des Tabletestes");
        }

        public override void RunAction(Action action)
        {
            action();
        }

        public override void Stop()
        {
            Console.WriteLine("Ende des TableTest, bitte [Enter] Drücken zum fortfahren!");
        }
    }
}
