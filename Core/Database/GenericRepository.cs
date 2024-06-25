using Newtonsoft.Json;
using Core.Config;
using Core.Logging;
using Core.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Interfaces;

namespace Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly string _filePath;
        private readonly Logger _logger;
        private readonly object _lock = new object();
        private readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();

        public GenericRepository()
        {
            var dataPath = ConfigLoader.Instance.GetValue("dataFilePath", "DatabaseConfig.json");
            if (string.IsNullOrWhiteSpace(dataPath))
            {
                throw new ArgumentException("The data file path cannot be null or empty. Check the configuration.");
            }

            _filePath = Path.Combine(dataPath, $"{typeof(T).Name}.json");
            _logger = Logger.Instance;

            if (!Directory.Exists(Path.GetDirectoryName(_filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
                _logger.Log(LogLevel.Info, $"Created directory: {Path.GetDirectoryName(_filePath)}");
            }

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
                _logger.Log(LogLevel.Info, $"Created file: {_filePath}");
            }
        }

        public void Create(T item)
        {
            lock (_lock)
            {
                _queue.Enqueue(() =>
                {
                    var items = LoadItems();
                    item.ID = items.Any() ? items.Max(i => i.ID) + 1 : 1;
                    items?.Add(item);
                    SaveItems(items??new List<T>());
                    _logger.Log(LogLevel.Info, $"Created item: {item.ID} :: {typeof(T).Name}");
                });
            }
        }

        public T Read(int id)
        {
            lock (_lock)
            {
                var items = LoadItems();
                return items.FirstOrDefault(i => i.ID == id);
            }
        }

        public void Update(T item)
        {
            lock (_lock)
            {
                _queue.Enqueue(() =>
                {
                    var items = LoadItems() ?? new List<T>();
                    var index = items.FindIndex(i => i.ID == item.ID);
                    if (index >= 0)
                    {
                        items[index] = item;
                        SaveItems(items);
                        _logger.Log(LogLevel.Info, $"Updated item: {item.ID} :: {typeof(T).Name}");
                    }
                });
            }
        }

        public void Delete(int id)
        {
            lock (_lock)
            {
                _queue.Enqueue(() =>
                {
                    var items = LoadItems() ?? new List<T>();
                    var item = items.FirstOrDefault(i => i.ID == id);
                    if (item != null)
                    {
                        items.Remove(item);
                        SaveItems(items);
                        _logger.Log(LogLevel.Info, $"Deleted item: {item.ID} :: {typeof(T).Name}");
                    }
                });
            }
        }

        public List<T> GetAll()
        {
            lock (_lock)
            {
                return LoadItems()??new List<T>();
            }
        }

        public void ResetIds()
        {
            lock (_lock)
            {
                _queue.Enqueue(() =>
                {
                    var items = LoadItems();
                    for (int i = 0; i < items?.Count; i++)
                    {
                        items[i].ID = i + 1;
                    }
                    if (items?.Count > 0)
                    {
                        SaveItems(items);
                        _logger.Log(LogLevel.Info, $"Reset IDs for all items of type: {typeof(T).Name}");
                    }
                });
            }
        }

        public void ProcessQueue()
        {
            lock (_lock)
            {
                while (_queue.TryDequeue(out var action))
                {
                    action();
                }
            }
        }

        private List<T>? LoadItems()
        {
            var json = File.ReadAllText(_filePath);
            if (json != null)
            {
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
            else return new List<T>();
        }

        private void SaveItems(List<T> items)
        {
            var json = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
