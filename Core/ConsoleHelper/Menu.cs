using System;
using System.Collections.Generic;
using System.Linq;
using Core.Config;
using Core.Interfaces;
using Core.Logging;

namespace Core.ConsoleHelper
{
    public static class Menu
    {
        public class MenuItem
        {
            public char Key { get; set; } = '\0';
            public string Description { get; set; } = string.Empty;
            public Action? Action { get; set; }
            public bool IsEmptyLine { get; set; } = false;

            public MenuItem(char key, string description, Action? action = null)
            {
                Key = key;
                Description = description;
                Action = action;
                IsEmptyLine = false;
            }

            public MenuItem(bool isEmptyLine)
            {
                IsEmptyLine = isEmptyLine;
            }
        }

        private static readonly int _maxSubMenuDepth;
        private static readonly char _menuDelimiter;
        private static readonly ConsoleColor _titleColor;
        private static readonly ConsoleColor _keyColor;
        private static readonly ConsoleColor _descriptionColor;
        private static readonly ConsoleColor _delimiterColor;
        private static readonly Logger _logger = Logger.Instance;

        public static bool isRunning = true;

        static Menu()
        {
            // Laden der Konfiguration
            _maxSubMenuDepth = int.Parse(ConfigLoader.Instance.GetValue("SubMenuMaxDepth", "MenuConfig.json"));
            _menuDelimiter = ConfigLoader.Instance.GetValue("MenuDelimiter", "MenuConfig.json")[0];
            _titleColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ConfigLoader.Instance.GetValue("TitleColor", "MenuConfig.json"));
            _keyColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ConfigLoader.Instance.GetValue("KeyColor", "MenuConfig.json"));
            _descriptionColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ConfigLoader.Instance.GetValue("DescriptionColor", "MenuConfig.json"));
            _delimiterColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ConfigLoader.Instance.GetValue("DelimiterColor", "MenuConfig.json"));
        }

        public static void ShowMenu(string title, List<MenuItem> items, int currentDepth = 0)
        {
            
            if (currentDepth > _maxSubMenuDepth)
            {
                _logger.Log(LogLevel.Err, "Maximale Tiefe der SubMenüs erreicht.");
                return;
            }

            while (isRunning)
            {
                Console.Clear();
                PrintCenteredTitle(title);
                foreach (var item in items)
                {
                    if (item.IsEmptyLine)
                    {
                        PrintEmptyLine();
                    }
                    else
                    {
                        PrintMenuItem(item);
                    }
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();
                var selectedItem = items.FirstOrDefault(i => !i.IsEmptyLine && i.Key == keyInfo.KeyChar);

                if (selectedItem != null)
                {
                    try
                    {
                        selectedItem.Action?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(LogLevel.Err, $"Fehler bei der Ausführung der Aktion: {ex.Message}");
                        break;
                    }
                }
                else
                {
                    _logger.Log(LogLevel.Err, "Ungültige Auswahl getroffen.");
                    break;
                }
            }
        }

        private static void PrintCenteredTitle(string title)
        {
            var centeredTitle = title.PadLeft((Console.WindowWidth + title.Length) / 2).PadRight(Console.WindowWidth);
            Console.ForegroundColor = _titleColor;
            Console.WriteLine(centeredTitle);
            Console.ResetColor();
        }

        private static void PrintEmptyLine()
        {
            Console.ForegroundColor = _delimiterColor;
            Console.WriteLine($"{_menuDelimiter}{new string(' ', Console.WindowWidth - 3)}{_menuDelimiter}");
            Console.ResetColor();
        }

        private static void PrintMenuItem(MenuItem item)
        {
            //string formattedItem = $"{_menuDelimiter}  {item.Key}  {item.Description.PadRight(Console.WindowWidth - 8)}{_menuDelimiter}";
            //Logger.Instance.Log(LogLevel.Debug, formattedItem);
            Console.ForegroundColor = _delimiterColor;
            Console.Write($"{_menuDelimiter}  ");
            Console.ForegroundColor = _keyColor;
            Console.Write($"{item.Key}  ");
            Console.ForegroundColor = _descriptionColor;
            Console.Write($"{item.Description.PadRight(Console.WindowWidth - 8)}");
            Console.ForegroundColor = _delimiterColor;
            Console.WriteLine(_menuDelimiter);
            Console.ResetColor();
        }
    }
}