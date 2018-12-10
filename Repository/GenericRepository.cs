using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using Common;
using AutoMapper;
using System.Reflection;

namespace Repository {
    public class GenericRepository<TEntity> where TEntity : Entity {

        internal readonly UserContext _context = null;
        internal readonly DbSet<TEntity> _dbSet = null;
        internal readonly IMapper _mapper = null;

        public GenericRepository(UserContext context, IMapper mapper) {
            _context = context ?? throw new ArgumentNullException("context");
            _dbSet = context.Set<TEntity>() ?? throw new ArgumentNullException("dbSet");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
        }


        public virtual async Task<bool> IsExistsAsync(
            Expression<Func<TEntity, bool>> filter = null) {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null) {
                return await query.AnyAsync(filter);
            }
            return false;
        }

        public virtual IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "") {
            IQueryable<TEntity> query = _dbSet;

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

        public virtual async Task<TEntity> GetAsync(bool eager = false,
           Expression<Func<TEntity, bool>> filter = null) {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null) {
                if (eager) {
                    foreach (var property in
                        _context.Model.FindEntityType(typeof(TEntity))
                        .GetNavigations()) {
                        query = query.Include(property.Name);
                    }
                }
                return await query.Where(filter).FirstOrDefaultAsync();
            }

            return null;
        }
        public virtual TEntity GetByID(object id) {

            return _dbSet.Find(id);
        }


        public virtual void Insert(TEntity entity) {
            _dbSet.Add(entity);
        }

        public virtual async Task<bool> Delete(object id) {
            TEntity entityToDelete = _dbSet.Find(id);
            return await DeleteAsync(entityToDelete);
        }

        public virtual async Task<bool>
            DeleteAsync(TEntity entityToDelete) {
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
                return false;
            }
        }

        public virtual async Task UpdateAsync(TEntity entityToUpdate) {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<TEntity> SaveAsync(TEntity model) {
            try {

                var mappingModel = _mapper.Map<TEntity>(model);

                var modelToSave = _dbSet.Add(mappingModel).Entity;
                //update db
                await _context.SaveChangesAsync();

                return modelToSave;

            } catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<int> FindIdByBkAsync(
            Expression<Func<TEntity, bool>> filter = null) {

            IQueryable<TEntity> query = _dbSet;

            TEntity entity = await query
                .Where(filter).FirstOrDefaultAsync();

            if (entity != null) {
                return entity.Id;
            }
            return 0;
        }

        public static bool IsItNew(DbContext context,
            object entity)
            => !context.Entry(entity).IsKeySet;
    }
}