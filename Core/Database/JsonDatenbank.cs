using Core.Interface;
using Core.Model;
using Core.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Database
{
    public class JsonDatabase<T> : BaseDatabase<T>, IDatabase<T> where T : BaseModel, new()
    {
        public JsonDatabase() : base()
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
                LoggingManager.LogMessage($"Saved {items.Count} items to JSON database");
            }
            catch (Exception ex)
            {
                LoggingManager.LogError($"Error saving JSON database: {ex.Message}", nameof(Save), 0);
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
                    var items = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
                    LoggingManager.LogMessage($"Loaded {items.Count} items from JSON database");
                    return items;
                }
                else
                {
                    LoggingManager.LogMessage($"JSON database file '{FullPath}' not found. Returning empty list.");
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.LogError($"Error loading JSON database: {ex.Message}", nameof(Load), 0);
                throw;
            }
        }
    }
}