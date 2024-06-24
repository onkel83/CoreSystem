// Core/Database/BaseDatabase.cs
using System;
using System.Collections.Generic;
using System.IO;
using Core.Interface;
using Core.Model;
using Core.Log;
using Core.Manager;

namespace Core.Database
{
    public abstract class BaseDatabase<T> : IDatabase<T> where T : BaseModel, new()
    {
        private string _fileName;
        private string _folderName;
        private int _lastID = 0;

        public string FileName
        {
            get => _fileName;
            set => _fileName = value;
        }

        public string FolderName
        {
            get => _folderName;
            set => _folderName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value);
        }

        public string FullPath => Path.Combine(FolderName, FileName);
        public string LastID
        {
            get => _lastID.ToString();
            set => _lastID = Convert.ToInt32(value);
        }

        protected BaseDatabase()
        {
            _fileName = ConfigManager.GetConfigValue("DatabaseFileName");
            _folderName = ConfigManager.GetConfigValue("DatabaseFolderName");
            EnsureFolderExists(FolderName);
            LoadIDs();
            LoggingManager.LogMessage($"Database initialized with FileName: {FileName} and FolderName: {FolderName}");
        }

        public abstract List<T> Load();
        public abstract void Save(List<T> items);

        public void Add(T item)
        {
            List<T> items = Load();
            item.ID = ++_lastID;
            items.Add(item);
            Save(items);
            LoggingManager.LogMessage($"Item added with ID: {item.ID}");
        }

        public void Delete(string ID)
        {
            List<T> items = Load();
            T itemToRemove = items.Find(x => x.ID.ToString() == ID);
            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
                ResetIDs(items);
                Save(items);
                LoggingManager.LogMessage($"Item deleted with ID: {ID}");
            }
        }

        public void Update(T updatedItem)
        {
            List<T> items = Load();
            int index = items.FindIndex(x => x.ID == updatedItem.ID);
            if (index != -1)
            {
                items[index] = updatedItem;
                Save(items);
                LoggingManager.LogMessage($"Item updated with ID: {updatedItem.ID}");
            }
        }

        protected void EnsureFolderExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }

        private void LoadIDs()
        {
            List<T> items = Load();
            if (items.Count > 0)
            {
                _lastID = items[^1].ID;
            }
            LoggingManager.LogMessage($"Loaded last ID: {_lastID}");
        }

        private void ResetIDs(List<T> items)
        {
            int newID = 1;
            foreach (var item in items)
            {
                item.ID = newID++;
            }
            _lastID = newID - 1;
            LoggingManager.LogMessage("Reset IDs for items");
        }
    }
}
