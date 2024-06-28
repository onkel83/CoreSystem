using Core.ConsoleHelper;
using System.Collections.Generic;
using WTA_Console.Utilities;

namespace WTA_Console
{
    public class MainMenu
    {
        private WorkerManager _wM = new WorkerManager();
        private WorkTimeManager _wTM = new WorkTimeManager();
        public MainMenu() { }

        public void Run()
        {
            List<Menu.MenuItem> menuItems = new List<Menu.MenuItem>
            {
                new Menu.MenuItem('1', "Arbeitszeit Verwaltung", ShowWorkTimeMenu),
                new Menu.MenuItem('2', "Mitarbeiter Verwaltung", ShowWorkerMenu),
                new Menu.MenuItem(true),
                new Menu.MenuItem('q', "Zurück zum Hauptmen", Stop),
            };
            Menu.ShowMenu("Arbeitszeit Verwaltung", menuItems);

        }
        private void Stop()
        {
            Menu.isRunning = false;
        }
        private void ShowWorkTimeMenu()
        {
            _wTM.Run();
        }
        private void ShowWorkerMenu()
        {
            _wM.Run();
        }
    }
}
