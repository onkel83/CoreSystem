using Core.Config;
using Core.ConsoleHelper;
using Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Core.TestCase
{
    public class MenuTest : TestBase
    {
        //private Logger _log = Logger.Instance;
        //private ConfigLoader _conf = ConfigLoader.Instance;

        private static MenuTest _Instance = new MenuTest();
        public static MenuTest Instance { get => _Instance ?? new MenuTest(); private set => _Instance = value; }

        private MenuTest()
        {
        }

        public override void Start()
        {
            Console.WriteLine("Start des Menutests");
        }

        public override void RunAction(Action action)
        {
            action();
        }

        public override void Stop()
        {
            Console.WriteLine("Ende des Menutests, bitte drücken Sie [Enter]");
            Console.ReadLine();
        }

        public void RunMenuTests()
        {
            List<Menu.MenuItem> mainMenuItems = new List<Menu.MenuItem>
            {
                new Menu.MenuItem('1', "Option 1", () => Console.WriteLine("Option 1 selected")),
                new Menu.MenuItem('2', "Option 2", () => Console.WriteLine("Option 2 selected")),
                new Menu.MenuItem(true), // Leerzeile
                new Menu.MenuItem('q', "Zurück zum Vorherigen Menu", () => Stop())
            };
            Menu.ShowMenu("Hauptmenü", mainMenuItems);
        }
    }
}
