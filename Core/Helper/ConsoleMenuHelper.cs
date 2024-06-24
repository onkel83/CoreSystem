using System;
using System.Collections.Generic;
using System.Linq;
using Core.Model;

namespace Core.Helper
{
    public static class ConsoleMenuHelper
    {
        public static void ShowMenu(string title, List<MenueEntry> entries, char borderChar = '#')
        {
            while (true)
            {
                PrintMenu(title, entries, borderChar);
                Console.Write("Ihre Auswahl Bitte: ");
                var input = Console.ReadKey().KeyChar;
                Console.WriteLine();
                var selectedEntry = entries.FirstOrDefault(e => e.Key == input);

                if (selectedEntry != null)
                {
                    try
                    {
                        selectedEntry?.Empfaenger?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        PrintError($"Fehler bei der Ausführung: {ex.Message}");
                    }
                }
                else
                {
                    PrintError("Ungültige Auswahl! Bitte versuchen Sie es erneut.");
                }
            }
        }

        private static void PrintMenu(string title, List<MenueEntry> entries, char borderChar)
        {
            Console.Clear();
            int maxWidth = Math.Max(title.Length, entries.Where(e => e.Description != null).Max(e => e.Description.Length) + 7);
            string borderLine = new string(borderChar, maxWidth + 4);
            Console.WriteLine(borderLine);
            Console.WriteLine($"{borderChar} {title.PadLeft((maxWidth + title.Length) / 2).PadRight(maxWidth)} {borderChar}");
            Console.WriteLine(borderLine);
            foreach (var entry in entries)
            {
                if (entry.Key == '\0' && entry.Description == null)
                {
                    Console.WriteLine($"{borderChar} {new string(' ', maxWidth)} {borderChar}");
                }
                else
                {
                    string line = $"{borderChar} {entry.Key} {borderChar} {entry?.Description?.PadRight(maxWidth - 4)} {borderChar}";
                    Console.WriteLine(line);
                }
            }
            Console.WriteLine(borderLine);
        }

        private static void PrintError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ResetColor();
            Console.WriteLine("Drücken Sie eine beliebige Taste, um fortzufahren...");
            Console.ReadKey();
        }
    }
}