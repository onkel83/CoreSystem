using Core.Interface;
using Core.Model;
using Core.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

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
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                using TextWriter writer = new StreamWriter(FullPath);
                serializer.Serialize(writer, items);
                writer.Flush();
                writer.Close();
                LoggingManager.LogMessage($"Saved {items.Count} items to XML database");
            }
            catch (Exception ex)
            {
                LoggingManager.LogError($"Error saving XML database: {ex.Message}", nameof(Save), 0);
                throw;
            }
        }

        public override List<T> Load()
        {
            try
            {
                if (File.Exists(FullPath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                    using TextReader reader = new StreamReader(FullPath);
                    var items = (List<T>)serializer.Deserialize(reader);
                    LoggingManager.LogMessage($"Loaded {items.Count} items from XML database");
                    return items;
                }
                else
                {
                    LoggingManager.LogMessage($"XML database file '{FullPath}' not found. Returning empty list.");
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.LogError($"Error loading XML database: {ex.Message}", nameof(Load), 0);
                throw;
            }
        }
    }
}