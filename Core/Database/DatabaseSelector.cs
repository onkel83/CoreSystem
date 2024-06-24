using Core.Interface;
using Core.Model;
using Core.Log;
using System;
using System.Collections.Generic;
using System.IO;
using Core.Manager;
using Core.Helper;

namespace Core.Database
{
    public class DatabaseSelector<T> where T : BaseModel, new()
    {
        private readonly IDatabase<T> _database;

        public DatabaseSelector(DatabaseType type)
        {
            string fileName = ConfigManager.GetConfigValue("DatabaseFileName");
            _database = type switch
            {
                DatabaseType.Xml => (IDatabase<T>)new XmlDatabase<T>(),
                DatabaseType.Json => (IDatabase<T>)new JsonDatabase<T>(),
                DatabaseType.Binary => (IDatabase<T>)new BinaryDatabase<T>(),
                _ => throw new ArgumentException("Unsupported database type", nameof(type)),
            };
            LoggingHelper.LogMessage($"DatabaseSelector initialized with type: {type} and file: {fileName}");
        }

        public void Save(List<T> items)
        {
            _database.Save(items);
            LoggingHelper.LogMessage($"Saved {items.Count} items");
        }

        public List<T> Load()
        {
            var items = _database.Load();
            LoggingHelper.LogMessage($"Loaded {items.Count} items");
            return items;
        }

        public void Add(T item)
        {
            _database.Add(item);
            LoggingHelper.LogMessage($"Added item with ID: {item.ID}");
        }

        public void Delete(string ID)
        {
            _database.Delete(ID);
            LoggingHelper.LogMessage($"Deleted item with ID: {ID}");
        }

        public void Update(T updatedItem)
        {
            _database.Update(updatedItem);
            LoggingHelper.LogMessage($"Updated item with ID: {updatedItem.ID}");
        }
    }

    public enum DatabaseType
    {
        Xml,
        Json,
        Binary
    }
}