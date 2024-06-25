using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        void Create(T item);
        void Delete(int id);
        List<T> GetAll();
        void ProcessQueue();
        T Read(int id);
        void ResetIds();
        void Update(T item);
    }
}