using Core.Interface;
using Core.Model;
using Core.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Core.Helper;

namespace Core.Database
{
    public class XmlDatabase<T> : BaseDatabase<T>, IDatabase<T> where T : BaseModel, new()
    {
        public XmlDatabase() : base()
        {
            EnsureFolderExists(Path.GetDirectoryName(FullPath));
        }

        public override void Save(List<T> items)
        {
            try
            {
                EnsureFolderExists(FolderName);
                FileHelper.SaveXmlFile(FullPath, items);
                LoggingHelper.LogMessage($"Saved {items.Count} items to Xmldatabase");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error saving Xmldatabase: {ex.Message}", nameof(Save), 19);
                throw;
            }
        }

        public override List<T> Load()
        {
            try
            {
                if (File.Exists(FullPath))
                {
                    var items = FileHelper.LoadXmlFile<T>(FullPath);
                    LoggingHelper.LogMessage($"Loaded {items.Count} items from Xmldatabase");
                    return items;
                }
                else
                {
                    LoggingHelper.LogMessage($"Xmldatabase file '{FullPath}' not found. Returning empty list.");
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError($"Error loading Xmldatabase: {ex.Message}", nameof(Load), 38);
                throw;
            }
        }
    }
}
