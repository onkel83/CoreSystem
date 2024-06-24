using Core.Interface;
using Core.Model;
using Core.Log;
using System;
using System.Collections.Generic;
using System.IO;
using Core.Manager;

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
            LoggingManager.LogMessage($"DatabaseSelector initialized with type: {type} and file: {fileName}");
        }

        public void Save(List<T> items)
        {
            _database.Save(items);
            LoggingManager.LogMessage($"Saved {items.Count} items");
        }

        public List<T> Load()
        {
            var items = _database.Load();
            LoggingManager.LogMessage($"Loaded {items.Count} items");
            return items;
        }

        public void Add(T item)
        {
            _database.Add(item);
            LoggingManager.LogMessage($"Added item with ID: {item.ID}");
        }

        public void Delete(string ID)
        {
            _database.Delete(ID);
            LoggingManager.LogMessage($"Deleted item with ID: {ID}");
        }

        public void Update(T updatedItem)
        {
            _database.Update(updatedItem);
            LoggingManager.LogMessage($"Updated item with ID: {updatedItem.ID}");
        }
    }

    public enum DatabaseType
    {
        Xml,
        Json,
        Binary
    }
}