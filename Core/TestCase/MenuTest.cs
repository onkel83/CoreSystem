using Core.Config;
using Core.ConsoleHelper;
using Core.Logging;
using System;
using System.Collections.Generic;

namespace Core.TestCase
{
    public class MenuTest : TestBase
    {
        private readonly Logger _log = Logger.Instance;
        private readonly ConfigLoader _conf = ConfigLoader.Instance;

        private static MenuTest _Instance = new MenuTest();
        public static MenuTest Instance { get => _Instance?? new MenuTest(); private set => _Instance = value; }

        private MenuTest()
        {
            List<Menu.MenuItem> mainMenuItems = new List<Menu.MenuItem>
            {
                new Menu.MenuItem('1', "Option 1", () => Console.WriteLine("Option 1 selected")),
                new Menu.MenuItem('2', "Option 2", () => Console.WriteLine("Option 2 selected")),
                new Menu.MenuItem(true), // Leerzeile
                new Menu.MenuItem('q', "Zurück zum Vorherigen Menu", () => Environment.Exit(0))
            };
            base.Run(() => Menu.ShowMenu("Hauptmenü", mainMenuItems));
        }

        public override void Start()
        {
            Console.WriteLine($"Start des Menutest");
        }

        public override void RunAction(Action action)
        {
            action();
        }

        public override void Stop()
        {
            Console.WriteLine($"Ende des Menutest, bitte drücken sie [Enter]");
            Console.ReadLine();
        }
    }
}
