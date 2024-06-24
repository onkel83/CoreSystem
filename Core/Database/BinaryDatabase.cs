using Core.Interface;
using Core.Model;
using Core.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Core.Helper;

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
                FileHelper.SaveBinaryFile(FullPath, items);
                LoggingHelper.LogMessage($"Saved {items.Count} items to binary database");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error saving binary database: {ex.Message}", nameof(Save), 20);
                throw;
            }
        }

        public override List<T> Load()
        {
            try
            {
                if (File.Exists(FullPath))
                {
                    var items = FileHelper.LoadBinaryFile<T>(FullPath);
                    LoggingHelper.LogMessage($"Loaded {items.Count} items from binary database");
                    return items;
                }
                else
                {
                    LoggingHelper.LogMessage($"Binary database file '{FullPath}' not found. Returning empty list.");
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error loading binary database: {ex.Message}", nameof(Load), 37);
                throw;
            }
        }
    }
}
