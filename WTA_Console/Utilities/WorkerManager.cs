using Core.Config;
using Core.ConsoleHelper;
using Core.Logging;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using WTA_Console.Models;

namespace WTA_Console.Utilities
{
    public class WorkerManager
    {
        private List<Worker> _Workers = new List<Worker>();
        private readonly ConfigLoader _Config = ConfigLoader.Instance;
        private readonly GenericRepository<Worker> _Repository = new GenericRepository<Worker>();

        public List<Worker> Workers { get => _Workers ?? new List<Worker>(); private set => _Workers = value; }

        public WorkerManager()
        {
            Workers = _Repository.GetAll();
        }

        public void Run()
        {
            CreateWorkerMenu();
        }

        private void Stop()
        {
            _Repository.ProcessQueue();
            _Repository.ResetIds();
            _Repository.ProcessQueue();
            Menu.isRunning = false;
        }

        private void CreateWorkerMenu()
        {
            List<Menu.MenuItem> menuItems = new List<Menu.MenuItem>
            {
                new Menu.MenuItem('1', "Neuen Eintrag anlegen", AddWorker),
                new Menu.MenuItem('2', "Eintrag bearbeiten", UpdateWorker),
                new Menu.MenuItem('3', "Eintrag löschen", DeleteWorker),
                new Menu.MenuItem('4', "Alle Einträge anzeigen", ShowAll),
                new Menu.MenuItem('5', "Einen Eintrag suchen", Search),
                new Menu.MenuItem(true),
                new Menu.MenuItem('8', "Speichern", Save),
                new Menu.MenuItem('9', "Reload", Reload),
                new Menu.MenuItem(true),
                new Menu.MenuItem('q', "Zurück zum Hauptmenü", Stop),
            };
            Menu.ShowMenu("Mitarbeiter Verwaltung", menuItems);
        }

        private void AddWorker()
        {
            var questions = new List<string>
            {
                "Bitte geben Sie den Nachnamen des Mitarbeiters ein:",
                "Bitte geben Sie den Vornamen des Mitarbeiters ein:"
            };
            var userInput = ConsoleHelper.UserNotNullInput(questions);
            var worker = new Worker { Name = userInput[0], Vorname = userInput[1] };
            Workers.Add(worker);
        }

        private void UpdateWorker()
        {
            var questions = new List<string> {
                "Bitte geben Sie den Nachnamen des Mitarbeiters ein: (Leer lassen, um alten Wert beizubehalten)",
                "Bitte geben Sie den Vornamen des Mitarbeiters ein: (Leer lassen, um alten Wert beizubehalten)"
            };
            var ID = ConsoleHelper.GetUserNotNullInput("Bitte geben Sie die ID des Eintrags ein:");
            var worker = Workers.Find(x => x.ID == Convert.ToInt32(ID));
            if (worker != null)
            {
                var answers = ConsoleHelper.UserInput(questions);
                if (answers != null && answers.Count >= 2)
                {
                    _Workers.Remove(worker);
                    worker.Name = string.IsNullOrEmpty(answers[0]) ? worker.Name : answers[0] ?? worker.Name;
                    worker.Vorname = string.IsNullOrEmpty(answers[1]) ? worker.Vorname : answers[1] ?? worker.Vorname;
                    _Workers.Add(worker);
                }
            }
            else
            {
                var answers = ConsoleHelper.UserNotNullInput(questions);
                if (answers != null && answers.Count >= 2)
                {
                    var newWorker = new Worker { Name = answers[0], Vorname = answers[1] };
                    _Workers.Add(newWorker);
                }
            }
        }

        private void DeleteWorker()
        {
            var ID = ConsoleHelper.GetUserNotNullInput("Bitte geben Sie die ID des Eintrags ein:");
            var worker = Workers.Find(x => x.ID == Convert.ToInt32(ID));
            if (worker != null)
            {
                _Workers.Remove(worker);
            }
        }

        private void ShowAll()
        {
            TableRenderer<Worker>.Render(Workers);
            Console.ReadLine();
        }

        private void Search()
        {
            List<Menu.MenuItem> menuItems = new List<Menu.MenuItem>
            {
                new Menu.MenuItem('1', "Nach Nachname", SearchName),
                new Menu.MenuItem('2', "Nach Vorname", SearchVorname),
                new Menu.MenuItem(true),
                new Menu.MenuItem('q', "Zurück zum Hauptmenü", CreateWorkerMenu),
            };
            Menu.ShowMenu("Mitarbeiter Suche", menuItems);
        }

        private void ShowSearch(List<Worker>? items, Predicate<Worker> predicate)
        {
            try
            {
                if (items != null && items.Count > 0)
                {
                    TableRenderer<Worker>.Render(SearchHelper.FindAll(predicate, items) ?? new List<Worker>());
                    ConsoleHelper.TableEnd();
                }
            }
            catch (Exception ex) { Catch(ex); }
        }

        private void SearchName()
        {
            try
            {
                var tmp = ConsoleHelper.GetUserInput("Bitte geben Sie den Nachnamen oder Teile des Nachnamens ein:");
                if (tmp != null && !string.IsNullOrEmpty(tmp))
                {
                    bool predicate(Worker w) => w.Name.Contains(tmp.Trim());
                    ShowSearch(_Repository.GetAll(), predicate);
                }
                else { ConsoleHelper.CancelByUser(); }
            }
            catch (Exception ex) { Catch(ex); }
        }

        private void SearchVorname()
        {
            try
            {
                var tmp = ConsoleHelper.GetUserInput("Bitte geben Sie den Vornamen oder Teile des Vornamens ein:");
                if (tmp != null && !string.IsNullOrEmpty(tmp))
                {
                    bool predicate(Worker w) => w.Vorname.Contains(tmp.Trim());
                    ShowSearch(_Repository.GetAll(), predicate);
                }
                else { ConsoleHelper.CancelByUser(); }
            }
            catch (Exception ex) { Catch(ex); }
        }

        private void Save()
        {
            try
            {
                var tmp = _Repository.GetAll();
                foreach (var item in Workers)
                {
                    if (tmp.Find(x => x.ID == item.ID) != default) { _Repository.Update(item); } else { _Repository.Create(item); }
                }
                _Repository.ProcessQueue();
                Logger.Instance.Log(Core.Interfaces.LogLevel.Info, "Mitarbeiter wurden gespeichert", $"{nameof(Save)}:{266}");
            }
            catch (Exception ex) { Catch(ex); }
        }

        private void Reload()
        {
            Workers.Clear();
            Workers = _Repository.GetAll();
            Logger.Instance.Log(Core.Interfaces.LogLevel.Info, "Mitarbeiter wurden neu geladen", $"{nameof(Reload)}:{279}");
        }

        private static void Catch(Exception ex)
        {
            Logger.Instance.Log(Core.Interfaces.LogLevel.Warn, ex.Message);
            Console.WriteLine(ex.Message);
            ConsoleHelper.TableEnd();
        }
    }
}
