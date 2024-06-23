using Core.Interface;
using Core.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Database
{
    public class DatabaseSelector<T> where T : BaseModel, new()
    {
        private readonly IDatabase<T> _database;
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        public DatabaseSelector(DatabaseType type, string fileName)
        {
            _database = type switch
            {
                DatabaseType.Xml => (IDatabase<T>)new XmlDatabase<T>(fileName, _filePath),
                DatabaseType.Json => (IDatabase<T>)new JsonDatabase<T>(fileName, _filePath),
                DatabaseType.Binary => (IDatabase<T>)new BinaryDatabase<T>(fileName, _filePath),
                _ => throw new ArgumentException("Unsupported database type", nameof(type)),
            };
        }

        public void Save(List<T> items)
        {
            _database.Save(items);
        }

        public List<T> Load()
        {
            return _database.Load();
        }

        public void Add(T item)
        {
            _database.Add(item);
        }

        public void Delete(string ID)
        {
            _database.Delete(ID);
        }

        public void Update(T updatedItem)
        {
            _database.Update(updatedItem);
        }
    }

    public enum DatabaseType
    {
        Xml,
        Json,
        Binary
    }
}
