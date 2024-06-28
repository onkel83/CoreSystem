using Core.Config;
using Core.ConsoleHelper;
using Core.Logging;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WTA_Console.Models;

namespace WTA_Console.Utilities
{
    public class WorkTimeManager
    {
        private List<WorkTime> _WorkTimes = new List<WorkTime>();
        private readonly ConfigLoader _Config = ConfigLoader.Instance;
        private readonly GenericRepository<WorkTime> _Repository = new GenericRepository<WorkTime>();


        public List<WorkTime> WorkTimes { get => _WorkTimes??new List<WorkTime>(); private set => _WorkTimes = value; }

        public WorkTimeManager()
        {
            WorkTimes = _Repository.GetAll();

        }

        public void Run()
        {
            CreateWorkTimeMenu();
        }
        private void Stop()
        {
            _Repository.ProcessQueue();
            _Repository.ResetIds();
            _Repository.ProcessQueue();
            Menu.isRunning = false;
        }

        private void CreateWorkTimeMenu()
        {
            List<Menu.MenuItem> menuItems = new List<Menu.MenuItem>
            {
                new Menu.MenuItem('1', "Neuen Eintrag anlegen", AddWorkTime),
                new Menu.MenuItem('2', "Eintrag bearbeiten", UpdateWorkTime),
                new Menu.MenuItem('3', "Eintrag löschen", DeleteWorkTime),
                new Menu.MenuItem('4', "Alle Einträge anzeigen", ShowAll),
                new Menu.MenuItem('5', "Einen Eintrag suchen", Search),
                new Menu.MenuItem(true),
                new Menu.MenuItem('8', "Speichern", Save),
                new Menu.MenuItem('9', "Reload", Reload),
                new Menu.MenuItem(true),
                new Menu.MenuItem('q', "Zurück zum Hauptmen", Stop),
            };
            Menu.ShowMenu("Arbeitszeit Verwaltung", menuItems);
        }

        private void AddWorkTime()
        {
            var questions = new List<string>
            {
                "Bitte geben Sie den Namen des Arbeitnehmers ein:",
                "Bitte geben Sie den Arbeitsevent ein:",
                "Bitte geben Sie den Arbeitsbeginn ein: (Format: [HH:mm dd.MM.YYYY])",
                "Bitte geben Sie den Arbeitsende ein: (Format: [HH:mm dd.MM.YYYY])",
                "Bitte geben Sie die Pausenzeit ein : (Format: 0.0 [Dezimal Stunden])"
            };
            var userInput = ConsoleHelper.UserNotNullInput(questions);
            var workTime = new WorkTime { Worker = userInput[0], Event = userInput[1], Start = userInput[2], Ende = userInput[3], Pause = userInput[4] };
            WorkTimes.Add(workTime);
        }
        private void UpdateWorkTime()
        {
            var questions = new List<string> {
                "Bitte geben sie den Namen des Arbeitnehmers ein : (Leer lassen um alten Wert bei zubehalten)",
                "Bitte geben sie den Event ein : (Leer lassen um alten Wert bei zubehalten)",
                "Bitte geben Sie den Arbeitsbeginn ein: (Format: [HH:mm dd.MM.YYYY])(Leer lassen um alten Wert bei zubehalten)",
                "Bitte geben Sie den Arbeitsende ein: (Format: [HH:mm dd.MM.YYYY])(Leer lassen um alten Wert bei zubehalten)",
                "Bitte geben Sie die Pausenzeit ein : (Format: 0.0 [Dezimal Stunden])(Leer lassen um alten Wert bei zubehalten)"
            };
            var ID = ConsoleHelper.GetUserNotNullInput("Bitte geben Sie die ID des Eintrags ein :");
            var wt = WorkTimes.Find(x => x.ID == Convert.ToInt32(ID));
            if (wt != null)
            {
                var answers = ConsoleHelper.UserInput(questions);
                if (answers != null && answers.Count >= 5)
                {
                    _WorkTimes.Remove(wt);
                    wt.Worker = string.IsNullOrEmpty(answers[0]) ? wt.Worker : answers[0]??wt.Worker;
                    wt.Event = string.IsNullOrEmpty(answers[1]) ? wt.Event : answers[1]??wt.Event;
                    wt.Start = string.IsNullOrEmpty(answers[2]) ? wt.Start : answers[2]??wt.Start;
                    wt.Ende = string.IsNullOrEmpty(answers[3]) ? wt.Ende : answers[3]??wt.Ende;
                    wt.Pause = string.IsNullOrEmpty(answers[4]) ? wt.Pause : answers[4]??wt.Pause;
                    _WorkTimes.Add(wt);

                }
            }
            else
            {
                var answers = ConsoleHelper.UserNotNullInput(questions);
                if (answers!= null && answers.Count >= 5)
                {
                    var workTime = new WorkTime { Worker = answers[0], Event = answers[1], Start = answers[2], Ende = answers[3], Pause = answers[4] };
                    _WorkTimes.Add(workTime);
                }
            }
        }
        private void DeleteWorkTime()
        {
            var ID = ConsoleHelper.GetUserNotNullInput("Bitte geben Sie die ID des Eintrags ein :");
            var wt = WorkTimes.Find(x => x.ID == Convert.ToInt32(ID));
            if (wt != null)
            {
                _WorkTimes.Remove(wt);
            }
        }
        private void ShowAll()
        {
            TableRenderer<WorkTime>.Render(WorkTimes);
            Console.ReadLine();
        }

        private void Search()
        {
            List<Menu.MenuItem> menuItems = new List<Menu.MenuItem>
            {
                new Menu.MenuItem('1', "Nach Name", SearchName),
                new Menu.MenuItem('2', "Nach Event", SearchEvent),
                new Menu.MenuItem('3', "Nach Startdatum", SearchStart),
                new Menu.MenuItem('4', "Nach Enddatum", SearchEnde),
                new Menu.MenuItem('5', "Nach Monat", SearchTime),
                new Menu.MenuItem('6', "Nach Jahr", SearchTime),
                new Menu.MenuItem(true),
                new Menu.MenuItem('8', "Auswertung nach Monat", GetWorkingHourMonth),
                new Menu.MenuItem('9', "Nach Jahr auswerten", GetWorkingHourYear),
                new Menu.MenuItem(true),
                new Menu.MenuItem('q', "Zurück zum Hauptmenu", Stop),
            };
            Menu.ShowMenu("Arbeitszeit Suche", menuItems);
        }
        private void ShowSearch(List<WorkTime>? items, Predicate<WorkTime> predicate)
        {
            try
            {
                if (items != null && items.Count > 0)
                {
                    TableRenderer<WorkTime>.Render(SearchHelper.FindAll(predicate, items)??new List<WorkTime>());
                    ConsoleHelper.TableEnd();
                }
            }
            catch (Exception ex) { Catch(ex); }
        }
        private void SearchName()
        {
            try {
                var tmp = ConsoleHelper.GetUserInput("Bitte geben sie den Name oder Teile des Namens ein : ");
                if (tmp != null && !string.IsNullOrEmpty(tmp))
                { bool predicate(WorkTime wt) => wt.Worker.Contains(tmp.Trim()); ShowSearch(_Repository.GetAll(), predicate); }
                else { ConsoleHelper.CancelByUser(); }
            }catch(Exception ex) { Catch(ex);}
        }
        private void SearchEvent()
        {
            try {
                var tmp = ConsoleHelper.GetUserInput("Bitte geben sie den Name oder Teile des Namens, des Events ein : ");
                if (tmp != null && !string.IsNullOrEmpty(tmp))
                    { bool predicate(WorkTime wt) => wt.Event.Contains(tmp.Trim()); ShowSearch(_Repository.GetAll(), predicate); }
                else { ConsoleHelper.CancelByUser(); }
            }catch(Exception ex) { Catch(ex); }
        }
        private void SearchStart()
        {
            try {
                var tmp = ConsoleHelper.GetUserInput(" Bitte geben sie das Startdatum ein : Format(dd.MM.YYYY)");
                if (tmp != null && !string.IsNullOrEmpty(tmp))
                { bool predicate(WorkTime wt) => wt.Start.Contains(tmp.Trim()); ShowSearch(_Repository.GetAll(), predicate); }
                else { ConsoleHelper.CancelByUser(); }
            }catch(Exception ex) { Catch(ex);}
        }
        private void SearchEnde()
        {
            try
            {
                var tmp = ConsoleHelper.GetUserInput(" Bitte geben sie das Enddatum ein : Format(dd.MM.YYYY)");
                if (tmp != null && !string.IsNullOrEmpty(tmp))
                { bool predicate(WorkTime wt) => wt.Ende.Contains(tmp.Trim()); ShowSearch(_Repository.GetAll(), predicate); }
                else { ConsoleHelper.CancelByUser(); }
            }
            catch (Exception ex){Catch(ex);}
        }
        private void SearchTime()
        {
            try
            {
                var tmp = ConsoleHelper.GetUserInput(" Bitte geben sie den Monat oder das Jahr in folgenden Formaten ein : (Monat : XX || Jahr : XXXX)");
                if (tmp != null && !string.IsNullOrEmpty(tmp))
                { bool predicate(WorkTime wt) => wt.Start.Contains(tmp.Trim()) || wt.Ende.Contains(tmp.Trim()); ShowSearch(_Repository.GetAll(), predicate); }
                else { ConsoleHelper.CancelByUser(); }
            }
            catch (Exception ex) { Catch(ex); }
        }
        private void GetWorkingHourMonth()
        {
            try
            {
                var tmp = ConsoleHelper.GetUserInput($"Bitte Geben sie den Monat ein : (Format : XX)");
                double result = 0;
                List<WorkTime> WorkingDays = new List<WorkTime>();
                if (tmp != null && !string.IsNullOrEmpty(tmp))
                {
                    foreach (WorkTime item in WorkTimes)
                    {
                        if (WorkTime.StringToDateTime(item.Start).Month == Convert.ToInt32(tmp))
                        {
                            result += item.TotalWorkTime;
                            WorkingDays.Add(item);
                        }
                    }
                }
                TableRenderer<WorkTime>.Render(WorkingDays);
                Console.WriteLine($"Arbeitsstunden im Monat : {result}");
                ConsoleHelper.TableEnd();
            }
            catch (Exception ex) { Catch(ex); }
        }
        private void GetWorkingHourYear()
        {
            try
            {

                var tmp = ConsoleHelper.GetUserInput("Bitte Geben sie das Jahr ein : (Format : XXX)");
                double result = 0;
                List<WorkTime> WorkingDays = new List<WorkTime>();
                if (tmp != null && !string.IsNullOrEmpty(tmp))
                {
                    foreach (WorkTime item in WorkTimes)
                    {
                        if (WorkTime.StringToDateTime(item.Start).Year == Convert.ToInt32(tmp))
                        {
                            result += item.TotalWorkTime;
                            WorkingDays.Add(item);
                        }
                    }
                }
                TableRenderer<WorkTime>.Render(WorkingDays);
                Console.WriteLine($"Arbeitsstunden im Jahr : {result}");
                ConsoleHelper.TableEnd();
            }catch (Exception ex){Catch(ex);}
        }

        private void Save()
        {
            try {
                var tmp = _Repository.GetAll();
                foreach (var item in WorkTimes)
                {
                    if (tmp.Find(x => x.ID == item.ID) != default) { _Repository.Update(item); } else { _Repository.Create(item); }
                }
                _Repository.ProcessQueue();
                Logger.Instance.Log(Core.Interfaces.LogLevel.Info, "Arbeitszeiten wurden gespeichert", $"{nameof(Save)}:{266}");
            }catch (Exception ex) { Catch(ex); }
        }
        private void Reload()
        {
            WorkTimes.Clear();WorkTimes = _Repository.GetAll();
            Logger.Instance.Log(Core.Interfaces.LogLevel.Info, "Arbeitszeiten wurden Reloadet", $"{nameof(Reload)}:{279}");
        }

        private static void Catch(Exception ex)
        {
            Logger.Instance.Log(Core.Interfaces.LogLevel.Warn, ex.Message);
            Console.WriteLine(ex.Message);ConsoleHelper.TableEnd();
        }
    }
}
