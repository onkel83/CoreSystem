using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Config;
using Core.Interfaces;
using Core.Logging;
using Core.Models;

namespace Core.ConsoleHelper
{
    public static class TableRenderer<T> where T : BaseModel
    {
        private static readonly Logger _logger = Logger.Instance;

        private static readonly ConsoleColor _headerColor;
        private static readonly ConsoleColor _cellColor;
        private static readonly ConsoleColor _alternateRowColor;
        private static readonly ConsoleColor _frameColor;

        static TableRenderer()
        {
            _headerColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ConfigLoader.Instance.GetValue("HeaderColor", "TableConfig.json"));
            _cellColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ConfigLoader.Instance.GetValue("CellColor", "TableConfig.json"));
            _alternateRowColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ConfigLoader.Instance.GetValue("AlternateRowColor", "TableConfig.json"));
            _frameColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ConfigLoader.Instance.GetValue("FrameColor", "TableConfig.json"));
        }

        public static void Render(List<T> items)
        {
            try
            {
                if (items == null || items.Count == 0)
                {
                    Console.WriteLine("Keine Daten vorhanden.");
                    return;
                }

                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead).ToList();

                var columnWidths = CalculateColumnWidths(properties, items);

                RenderHeader(properties, columnWidths);
                RenderRows(items, properties, columnWidths);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Err, $"Fehler beim Rendern der Tabelle: {ex.Message}");
            }
        }

        private static List<int> CalculateColumnWidths(List<PropertyInfo> properties, List<T> items)
        {
            var maxColumnWidths = new List<int>();

            foreach (var property in properties)
            {
                var maxLength = property.Name.Length;

                foreach (var item in items)
                {
                    var value = property.GetValue(item)?.ToString() ?? string.Empty;
                    if (value.Length > maxLength)
                    {
                        maxLength = value.Length;
                    }
                }

                maxColumnWidths.Add(maxLength);
            }

            return maxColumnWidths;
        }

        private static void RenderHeader(List<PropertyInfo> properties, List<int> columnWidths)
        {
            Console.ForegroundColor = _frameColor;
            Console.Write("╔");
            foreach (var width in columnWidths)
            {
                Console.Write(new string('═', width + 2));
                Console.Write("╦");
            }
            Console.WriteLine("╗");

            Console.ForegroundColor = _headerColor;
            Console.Write("║");
            for (int i = 0; i < properties.Count; i++)
            {
                var name = properties[i].Name;
                var paddedName = name.PadLeft((columnWidths[i] + name.Length) / 2).PadRight(columnWidths[i]);
                Console.Write($" {paddedName} ║");
            }
            Console.WriteLine();

            Console.ForegroundColor = _frameColor;
            Console.Write("╠");
            foreach (var width in columnWidths)
            {
                Console.Write(new string('═', width + 2));
                Console.Write("╬");
            }
            Console.WriteLine("╣");
        }

        private static void RenderRows(List<T> items, List<PropertyInfo> properties, List<int> columnWidths)
        {
            for (int rowIndex = 0; rowIndex < items.Count; rowIndex++)
            {
                var item = items[rowIndex];
                var color = rowIndex % 2 == 0 ? _cellColor : _alternateRowColor;
                Console.ForegroundColor = _frameColor;
                Console.Write("║");

                foreach (var property in properties)
                {
                    var value = property.GetValue(item)?.ToString() ?? string.Empty;
                    var paddedValue = value.PadRight(columnWidths[properties.IndexOf(property)]);
                    Console.ForegroundColor = color;
                    Console.Write($" {paddedValue} ║");
                }

                Console.WriteLine();
                Console.ForegroundColor = _frameColor;

                if (rowIndex < items.Count - 1)
                {
                    Console.Write("╟");
                    foreach (var width in columnWidths)
                    {
                        Console.Write(new string('─', width + 2));
                        Console.Write("╫");
                    }
                    Console.WriteLine("╢");
                }
                else
                {
                    Console.Write("╚");
                    foreach (var width in columnWidths)
                    {
                        Console.Write(new string('═', width + 2));
                        Console.Write("╩");
                    }
                    Console.WriteLine("╝");
                }
            }

            Console.ResetColor();
        }
    }
}
