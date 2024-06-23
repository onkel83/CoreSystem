using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Core.Helper
{
    public static class FileHelper
    {
        /// <summary>
        /// Speichert den angegebenen Text in einer Textdatei.
        /// </summary>
        /// <param name="filePath">Der Pfad zur Datei.</param>
        /// <param name="content">Der zu speichernde Textinhalt.</param>
        public static void SaveTextFile(string filePath, string content)
        {
            EnsureDirectoryExists(filePath);
            File.WriteAllText(filePath, content);
        }

        /// <summary>
        /// Lädt den Inhalt einer Textdatei.
        /// </summary>
        /// <param name="filePath">Der Pfad zur Datei.</param>
        /// <returns>Der Inhalt der Textdatei als Zeichenfolge.</returns>
        public static string LoadTextFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return string.Empty;
        }

        /// <summary>
        /// Speichert eine Liste von Objekten als JSON-Datei.
        /// </summary>
        /// <typeparam name="T">Der Typ der Objekte in der Liste.</typeparam>
        /// <param name="filePath">Der Pfad zur JSON-Datei.</param>
        /// <param name="items">Die Liste von Objekten, die gespeichert werden soll.</param>
        public static void SaveJsonFile<T>(string filePath, List<T> items)
        {
            string jsonData = JsonConvert.SerializeObject(items, Formatting.Indented);
            SaveTextFile(filePath, jsonData);
        }

        /// <summary>
        /// Lädt eine Liste von Objekten aus einer JSON-Datei.
        /// </summary>
        /// <typeparam name="T">Der Typ der Objekte in der Liste.</typeparam>
        /// <param name="filePath">Der Pfad zur JSON-Datei.</param>
        /// <returns>Die Liste von Objekten aus der JSON-Datei.</returns>
        public static List<T> LoadJsonFile<T>(string filePath)
        {
            string jsonData = LoadTextFile(filePath);
            if (!string.IsNullOrEmpty(jsonData))
            {
                return JsonConvert.DeserializeObject<List<T>>(jsonData)??new List<T>();
            }
            return new List<T>();
        }

        /// <summary>
        /// Speichert eine Liste von Objekten als JSON-Array in einer Binärdatei.
        /// </summary>
        /// <typeparam name="T">Der Typ der Objekte in der Liste.</typeparam>
        /// <param name="filePath">Der Pfad zur Binärdatei.</param>
        /// <param name="items">Die Liste von Objekten, die gespeichert werden soll.</param>
        public static void SaveBinaryFile<T>(string filePath, List<T> items)
        {
            string jsonData = JsonConvert.SerializeObject(items, Formatting.None);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonData);
            SaveBinaryData(filePath, data);
        }

        /// <summary>
        /// Lädt eine Liste von Objekten aus einer Binärdatei, die als JSON-Array gespeichert ist.
        /// </summary>
        /// <typeparam name="T">Der Typ der Objekte in der Liste.</typeparam>
        /// <param name="filePath">Der Pfad zur Binärdatei.</param>
        /// <returns>Die Liste von Objekten aus der Binärdatei.</returns>
        public static List<T> LoadBinaryFile<T>(string filePath)
        {
            byte[] data = LoadBinaryData(filePath);
            if (data != null && data.Length > 0)
            {
                string jsonData = System.Text.Encoding.UTF8.GetString(data);
                return JsonConvert.DeserializeObject<List<T>>(jsonData)??new List<T>();
            }
            return new List<T>();
        }

        /// <summary>
        /// Speichert binäre Daten in einer Binärdatei.
        /// </summary>
        /// <param name="filePath">Der Pfad zur Binärdatei.</param>
        /// <param name="data">Die zu speichernden binären Daten.</param>
        private static void SaveBinaryData(string filePath, byte[] data)
        {
            EnsureDirectoryExists(filePath);
            using FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// Lädt binäre Daten aus einer Binärdatei.
        /// </summary>
        /// <param name="filePath">Der Pfad zur Binärdatei.</param>
        /// <returns>Die geladenen binären Daten.</returns>
        private static byte[] LoadBinaryData(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }
            return new byte[0];
        }

        /// <summary>
        /// Überprüft, ob das Verzeichnis für die angegebene Datei existiert. Falls nicht, wird es erstellt.
        /// </summary>
        /// <param name="filePath">Der Pfad zur Datei.</param>
        private static void EnsureDirectoryExists(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
