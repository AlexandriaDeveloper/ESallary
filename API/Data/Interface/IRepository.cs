using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Helper;
using API.Models;

namespace API.Data.Interface {
    public interface IRepository<T> where T : class {
        void Add (T entity);
        void AddRange (List<T> entity);
        void Delete (T entity);

          void DeleteRange (List<T> entity) ;
        void Update (T entity);
        void UpdateRange (List<T> entities);
        Task<bool> SaveAll ();
        Task<IEnumerable<T>> Get ();
        Task<T> Get (int id);
        IQueryable<T> Get (Expression<Func<T, bool>> filter = null, string includes = null);
        Task<T> Get (string id);
        Task<IQueryable<T>> Get (EmpsParams empsParams, string includes,
            Func<IQueryable<T>, IOrderedQueryable<T>> predict = null, Expression<Func<T, bool>> filter = null);
        Task<IEnumerable<T>> Get (Expression<Func<T, bool>> query);
    }
}