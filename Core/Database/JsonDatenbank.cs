using Core.Interface;
using Core.Model;
using Core.Helper;
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
                FileHelper.SaveJsonFile(FullPath, items);
                LoggingHelper.LogMessage($"Saved {items.Count} items to JSON database");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error saving JSON database: {ex.Message}", nameof(Save), 17);
                throw;
            }
        }

        public override List<T> Load()
        {
            try
            {
                if (File.Exists(FullPath))
                {
                    var items = FileHelper.LoadJsonFile<T>(FullPath);
                    LoggingHelper.LogMessage($"Loaded {items.Count} items from JSON database");
                    return items;
                }
                else
                {
                    LoggingHelper.LogMessage($"JSON database file '{FullPath}' not found. Returning empty list.");
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error loading JSON database: {ex.Message}", nameof(Load), 32);
                throw;
            }
        }
    }
}
