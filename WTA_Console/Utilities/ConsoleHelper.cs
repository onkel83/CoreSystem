using System;
using System.Collections.Generic;
using System.Text;

namespace WTA_Console.Utilities
{
    public static class ConsoleHelper
    {
        public static List<string?> UserInput(List<string> promts)
        {
            List<string?> result = new List<string?>();
            foreach (string prom in promts)
            {
                result.Add(new string(GetUserInput(prom)));
            }
            return result;
        }
        public static List<string> UserNotNullInput(List<string> promts)
        {
            List<string> result = new List<string>();
            foreach(string prom in promts)
            {
                result.Add(GetUserNotNullInput(prom));
            }
            return result;
        }

        public static string? GetUserInput(string? prompt)
        {
            if (prompt != null) {
                Console.WriteLine(prompt);
                var tmp = Console.ReadLine();
                if (string.IsNullOrEmpty(tmp))
                {
                    return string.Empty;
                }
                else
                {
                    return tmp;
                }
            }return string.Empty;
        }

        public static string GetUserNotNullInput(string prompt)
        {
            int i = 0;
            while (true)
            {
                Console.WriteLine(prompt);
                var tmp = Console.ReadLine();
                if(!string.IsNullOrEmpty(tmp))
                {
                    return tmp;
                }
                i++;
                if(i >= 5)
                {
                    i = 0;
                    Console.Clear();
                }
            }
        }
        public static void CancelByUser()
        {
            Console.WriteLine();
            Console.WriteLine($"Abbruch durch Benutzer! Bitte [Enter] drücken zum fortfahren");
            Console.ReadLine();
        }

        public static void TableEnd(string? text = null)
        {
            string defaultValue = "Bitte[Enter] drücken zum fortfahren";
            Console.WriteLine($"{(string.IsNullOrEmpty(text) ? defaultValue : text)}");
            Console.ReadLine();
        }
    }
}
