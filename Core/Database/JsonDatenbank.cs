using Core.Interface;
using Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Database
{
    public class JsonDatabase<T> : BaseDatabase<T>, IDatabase<T> where T : BaseModel, new()
    {
        public JsonDatabase(string fileName, string folderName):base(fileName, folderName)
        {
            EnsureFolderExists(Path.GetDirectoryName(FullPath));
        }

        public override void Save(List<T> items)
        {
            try
            {
                EnsureFolderExists(FolderName);

                string json = JsonConvert.SerializeObject(items);
                File.WriteAllText(FullPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving JSON database: {ex.Message}");
                throw;
            }
        }

        public override List<T> Load()
        {
            try
            {
                if (File.Exists(FullPath))
                {
                    string json = File.ReadAllText(FullPath);
                    return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
                }
                else
                {
                    Console.WriteLine($"JSON database file '{FullPath}' not found. Returning empty list.");
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading JSON database: {ex.Message}");
                throw;
            }
        }
    }
}
