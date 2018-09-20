using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository {
    public class Repository<T> : IRepository<T> where T : class {
        private readonly DataContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository (DataContext context) {
            _context = context;
            _dbSet = _context.Set<T> ();

        }

        public virtual void Add (T entity) {
            _dbSet.Add (entity);
        }

        public void Delete (T entity) {
          if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Attach(entity);
            }
            _dbSet.Remove(entity);
        }
        public void Update (T entity) {
             _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        public virtual async Task<IEnumerable<T>> Get () {
            return await _dbSet.ToListAsync ();
        }

        public virtual Task<T> Get (int id) {
            return _dbSet.FindAsync (id);
        }

        public Task<bool> SaveAll () {
            throw new System.NotImplementedException ();
        }
    }
}