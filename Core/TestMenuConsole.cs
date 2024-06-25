using Core.ConsoleHelper;
using Core.TestCase;
using System;
using System.Collections.Generic;

namespace Core.Apps
{
    public static  class TestMenuConsole
    {
        public static void Show()
        {
            List<Menu.MenuItem> mainMenuItems = new List<Menu.MenuItem>
            {
                new Menu.MenuItem('1', "Logger Test", () => LoggerTest.Instance.Run(LoggerTest.Instance.RunLoggerTests)),
                new Menu.MenuItem('2', "ConfigLoader Test", () => ConfigLoaderTest.Instance.Run(ConfigLoaderTest.Instance.RunConfigLoaderTests)),
                new Menu.MenuItem('3', "GenericRepository Test", () => GenericRepositoryTest.Instance.Run(GenericRepositoryTest.Instance.RunGenericRepositoryTests)),
                new Menu.MenuItem('4', "Menu Test", () => MenuTest.Instance.Run(MenuTest.Instance.RunMenuTests)),
                new Menu.MenuItem('5', "Table Test", () => TableTest.Instance.Run(TableTest.Instance.RunTableTests)),
                new Menu.MenuItem(true), // Leerzeile
                new Menu.MenuItem('q', "Beenden", () => Environment.Exit(0))
            };

            Menu.ShowMenu("Test Menü", mainMenuItems);
        }
    }
}
