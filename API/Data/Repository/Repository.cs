using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Data.Interface;
using API.Helper;
using Microsoft.EntityFrameworkCore;
using NinjaNye.SearchExtensions;

namespace API.Data.Repository {
    public class Repository<T> : IRepository<T> where T : class {
        private readonly DataContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository (DataContext context) {
            _context = context;
            _dbSet = _context.Set<T> ();

        }

        public async virtual void Add (T entity) {
            await _dbSet.AddAsync (entity);
        }

        public virtual async void AddRange (List<T> entity) {
            await _dbSet.AddRangeAsync (entity);
        }
        public void Delete (T entity) {
            if (_context.Entry (entity).State == EntityState.Detached) {
                _context.Attach (entity);
            }
            _dbSet.Remove (entity);
        }
        public void Update (T entity) {
            // _dbSet.Attach (entity);
            // _context.Entry (entity).State = EntityState.Modified;
            _context.Update (entity);
        }

        public void UpdateRange (List<T> entities) {
            // _dbSet.Attach (entity);
            // _context.Entry (entity).State = EntityState.Modified;
            _context.UpdateRange (entities);
        }
        public virtual async Task<IEnumerable<T>> Get () {
            return await _dbSet.AsNoTracking ().ToListAsync ();
        }

        public virtual Task<T> Get (int id) {
            return _dbSet.FindAsync (id);
        }
        public virtual Task<T> Get (string id) {
            return _dbSet.FindAsync (id);
        }

        public virtual async Task<IQueryable<T>> Get (EmpsParams empsParams, string includes,
            Func<IQueryable<T>, IOrderedQueryable<T>> predict = null, Expression<Func<T, bool>> filter = null) {
            IQueryable<T> query;
            // query = _dbSet;
            if (filter != null) {
                query = _dbSet.Where (filter).AsNoTracking ();

            } else {
                query = _dbSet.AsNoTracking ();
            }
            if (predict != null) {
                query = predict (query);
            }

            var items = includes.Split (',');
            foreach (var item in items) {
                if (items.Length > 0 && !string.IsNullOrEmpty (item))
                    query = query.Include (item);
            }
            return query; //await PagedList<T>.CreateAsync (query, empsParams.PageNumber, empsParams.PageSize);
        }

        public virtual IQueryable<T> Get (Expression<Func<T, bool>> filter = null, string includes = null) {
            IQueryable<T> query;
            // query = _dbSet;
            if (filter != null) {
                query = _dbSet.Where (filter).AsNoTracking ();

            } else {
                query = _dbSet.AsNoTracking ();
            }

            var items = includes.Split (',');
            foreach (var item in items) {
                if (items.Length > 0)
                    query = query.Include (item);
            }
            return query;
        }
        public virtual async Task<IEnumerable<T>> Get (Expression<Func<T, bool>> query) {
            return await _dbSet.Where (query).AsNoTracking ().ToListAsync ();
        }

        public Task<bool> SaveAll () {
            throw new System.NotImplementedException ();
        }
    }
}