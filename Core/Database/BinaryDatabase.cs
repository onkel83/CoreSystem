using Core.Interface;
using Core.Model;
using Core.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Database
{
    public class BinaryDatabase<T> : BaseDatabase<T>, IDatabase<T> where T : BaseModel, new()
    {
        public BinaryDatabase() : base()
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
                LoggingManager.LogMessage($"Saved {items.Count} items to binary database");
            }
            catch (Exception ex)
            {
                LoggingManager.LogError($"Error saving binary database: {ex.Message}", nameof(Save), 0);
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
                    var items = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
                    LoggingManager.LogMessage($"Loaded {items.Count} items from binary database");
                    return items;
                }
                else
                {
                    LoggingManager.LogMessage($"Binary database file '{FullPath}' not found. Returning empty list.");
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.LogError($"Error loading binary database: {ex.Message}", nameof(Load), 0);
                throw;
            }
        }
    }
}