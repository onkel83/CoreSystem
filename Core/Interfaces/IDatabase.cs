using System.Collections.Generic;

namespace Core.Interface
{

    public interface IDatabase<T>
    {
        string LastID { get; set; }
        string FileName { get; set; }
        string FolderName { get; set; }
        string FullPath { get; }

        
        void Save(List<T> items);
        List<T> Load();
        void Delete(string ID);
        void Update(T updatedItem);
        void Add(T item);
    }
}
