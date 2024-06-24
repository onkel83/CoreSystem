using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Core.Helper
{
    public static class FileHelper
    {
        public static void SaveXmlFile<T>(string filePath, List<T> items)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using TextWriter writer = new StreamWriter(filePath);
            serializer.Serialize(writer, items);
        }
        public static List<T> LoadXmlFile<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using TextReader reader = new StreamReader(filePath);
            return (List<T>)serializer.Deserialize(reader);
        }

        public static void SaveTextFile(string filePath, string content)
        {
            EnsureDirectoryExists(filePath);
            File.WriteAllText(filePath, content);
        }

        public static string LoadTextFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return string.Empty;
        }

        public static void SaveJsonFile<T>(string filePath, List<T> items)
        {
            string jsonData = JsonConvert.SerializeObject(items, Formatting.Indented);
            SaveTextFile(filePath, jsonData);
        }

        public static List<T> LoadJsonFile<T>(string filePath)
        {
            string jsonData = LoadTextFile(filePath);
            if (!string.IsNullOrEmpty(jsonData))
            {
                return JsonConvert.DeserializeObject<List<T>>(jsonData) ?? new List<T>();
            }
            return new List<T>();
        }

        public static void SaveBinaryFile<T>(string filePath, List<T> items)
        {
            string jsonData = JsonConvert.SerializeObject(items, Formatting.None);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonData);
            SaveBinaryData(filePath, data);
        }

        public static List<T> LoadBinaryFile<T>(string filePath)
        {
            byte[] data = LoadBinaryData(filePath);
            if (data != null && data.Length > 0)
            {
                string jsonData = System.Text.Encoding.UTF8.GetString(data);
                return JsonConvert.DeserializeObject<List<T>>(jsonData) ?? new List<T>();
            }
            return new List<T>();
        }

        private static void SaveBinaryData(string filePath, byte[] data)
        {
            EnsureDirectoryExists(filePath);
            using FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }

        private static byte[] LoadBinaryData(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }
            return new byte[0];
        }

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
