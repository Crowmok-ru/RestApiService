using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiService.Context
{
    public interface IBasicRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        T GetCache(int id);
        void Create(T item);
        void Update(T item);
        T Delete(int id);
    }
}
