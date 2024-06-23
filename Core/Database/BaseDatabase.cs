using System;
using System.Collections.Generic;
using System.IO;
using Core.Interface;
using Core.Model;

namespace Core.Database
{
    public abstract class BaseDatabase<T> : IDatabase<T> where T : BaseModel, new()
    {
        private string _fileName = string.Empty;
        private string _folderName = string.Empty;
        private int _lastID = 0;

        public string FileName { get => _fileName; set => _fileName = value; }
        public string FolderName { get => _folderName; set => _folderName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value); }
        public string FullPath => Path.Combine(FolderName, FileName);
        public string LastID { get => _lastID.ToString(); set => _lastID = Convert.ToInt32(value); }

        protected BaseDatabase(string fileName, string folderName)
        {
            FileName = fileName;
            FolderName = folderName;
            EnsureFolderExists(FolderName);

            // Load IDs from existing items
            LoadIDs();
        }

        public abstract List<T> Load();

        public abstract void Save(List<T> items);

        public void Add(T item)
        {
            List<T> items = Load();
            item.ID = ++_lastID;
            items.Add(item);
            Save(items);
        }

        public void Delete(string ID)
        {
            List<T> items = Load();
            T itemToRemove = items.Find(x => x.ID.ToString() == ID);
            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
                ResetIDs(items); // Reset IDs after deletion
            }
            Save(items);
        }

        public void Update(T updatedItem)
        {
            List<T> items = Load();
            int index = items.FindIndex(x => x.ID == updatedItem.ID);
            if (index != -1)
                items[index] = updatedItem;
            Save(items);
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
                _lastID = items[^1].ID; // Set last ID to the highest existing ID
            }
        }

        private void ResetIDs(List<T> items)
        {
            // Reset IDs starting from 1
            int newID = 1;
            foreach (var item in items)
            {
                item.ID = newID++;
            }
            _lastID = newID - 1; // Set last ID to the highest new ID
        }
    }
}
