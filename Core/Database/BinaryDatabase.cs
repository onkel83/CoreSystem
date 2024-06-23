using Core.Interface;
using Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Database
{
    public class BinaryDatabase<T> : BaseDatabase<T>, IDatabase<T> where T : BaseModel, new()
    {
        public BinaryDatabase(string fileName, string folderName):base(fileName, folderName)
        {
            EnsureFolderExists(Path.GetDirectoryName(FullPath));
        }

        public override void Save(List<T> items)
        {
            try
            {
                EnsureFolderExists(FolderName);

                string json = JsonConvert.SerializeObject(items);
                byte[] data = Encoding.UTF8.GetBytes(json);
                File.WriteAllBytes(FullPath, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving database: {ex.Message}");
                throw;
            }
        }

        public override List<T> Load()
        {
            try
            {
                if (File.Exists(FullPath))
                {
                    byte[] data = File.ReadAllBytes(FullPath);
                    string json = Encoding.UTF8.GetString(data);
                    return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
                }
                else
                {
                    Console.WriteLine($"Database file '{FullPath}' not found. Returning empty list.");
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading database: {ex.Message}");
                throw;
            }
        }
    }
}
