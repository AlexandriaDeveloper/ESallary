using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Data.Interface {
    public interface IRepository<T> where T : class {
        void Add  (T entity);
        void Delete(T entity);
        Task<bool> SaveAll ();
        Task<IEnumerable<T>> Get ();
        Task<T> Get(int id);
    }
}