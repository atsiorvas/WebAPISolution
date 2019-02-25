using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using Common;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Common.Interface;

namespace Repository {
    public class GenericRepository<T> : IGenericRepository<T>
        where T : Entity, new() {

        private readonly UserContext _context = null;
        private readonly DbSet<T> _dbSet = null;
        private readonly IMapper _mapper = null;
        private readonly ILogger _logger = null;


        // parameterless constructor
        public GenericRepository() { }

        public GenericRepository(
            UserContext context,
            IMapper mapper,
            ILogger<GenericRepository<T>> logger) {
            _context = context ?? throw new ArgumentNullException("context");
            _dbSet = context.Set<T>() ?? throw new ArgumentNullException("dbSet");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
            _logger = logger ?? throw new ArgumentNullException("logger");
        }

        public GenericRepository(
           UserContext context,
           IMapper mapper) {
            _context = context ?? throw new ArgumentNullException("context");
            _dbSet = context.Set<T>() ?? throw new ArgumentNullException("dbSet");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
        }

        public virtual void AddOrUpdate(T entity) {

            _context.Entry(entity).State = entity.Id == 0 ?
                                EntityState.Added :
                                EntityState.Modified;
        }

        public virtual async Task<bool> IsExistsAsync(
            Expression<Func<T, bool>> filter = null) {
            IQueryable<T> query = _dbSet;
            if (filter != null) {
                return await query.AnyAsync(filter);
            }
            return false;
        }

        public virtual IEnumerable<T> Get(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "") {
            IQueryable<T> query = _dbSet;

            if (filter != null) {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                query = query.Include(includeProperty);
            }

            if (orderBy != null) {
                return orderBy(query).ToList();
            } else {
                return query.ToList();
            }
        }

        public virtual async Task<T> GetAsync(bool eager = false,
           Expression<Func<T, bool>> filter = null) {
            IQueryable<T> query = _dbSet;

            if (filter != null) {
                if (eager) {
                    foreach (var property in
                        _context.Model.FindEntityType(typeof(T))
                        .GetNavigations()) {
                        query = query.Include(property.Name);
                    }
                }
                return await query.Where(filter).FirstOrDefaultAsync();
            }
            return null;
        }

        public virtual IQueryable<T> GetQuery(bool eager = false,
           Expression<Func<T, bool>> filter = null) {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (filter != null) {
                if (eager) {
                    foreach (var property in
                        _context.Model.FindEntityType(typeof(T))
                        .GetNavigations()) {
                        query = query.Include(property.Name);
                    }
                }
                return query.Where(filter);
            }
            return null;
        }

        public virtual T GetByID(long id) {

            return _dbSet.Find(id);
        }

        public virtual void Insert(T entity) {
            _dbSet.Add(entity);
        }

        public virtual async Task<bool> Delete(long id) {
            T entityToDelete = _dbSet.Find(id);
            return await DeleteAsync(entityToDelete);
        }

        public virtual async Task<bool>
            DeleteAsync(T entityToDelete) {
            try {
                if (
                    _context.Entry(entityToDelete).State
                    == EntityState.Detached
                    ) {
                    _dbSet.Attach(entityToDelete);
                }
                _dbSet.Remove(entityToDelete);
                await _context.SaveChangesAsync();
                return true;
            } catch (Exception ex) {
                _logger.LogError("Exception: ", ex);
                return false;
            }
        }

        public virtual bool Update(T entityToUpdate) {
            try {
                _dbSet.Attach(entityToUpdate);
                _context.Entry(entityToUpdate).State = EntityState.Modified;
                return true;
            } catch (Exception ex) {
                _logger.LogError("Exception: ", ex);
                return false;
            }
        }

        public async Task<T> SaveAsync(T model) {
            try {
                var modelToSave = _dbSet.Add(model).Entity;
                //update db
                await _context.SaveChangesAsync();

                return modelToSave;

            } catch (Exception ex) {
                _logger.LogError("Exception", ex);
                return null;
            }
        }

        public async Task<long> FindIdByBkAsync(
            Expression<Func<T, bool>> filter = null) {

            IQueryable<T> query = _dbSet;

            T entity = await query
                .Where(filter).FirstOrDefaultAsync();

            if (entity != null) {
                return entity.Id;
            }
            return 0;
        }

        public bool IsItNew(DbContext context,
            object entity)
            => !context.Entry(entity).IsKeySet;
    }
}