using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Interface {
    public interface IGenericRepository<T>
        where T : Entity {

        void AddOrUpdate(T entity);
        Task<bool> IsExistsAsync(
            Expression<Func<T, bool>> filter = null);
        IEnumerable<T> Get(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "");
        Task<T> GetAsync(bool eager = false,
           Expression<Func<T, bool>> filter = null);

        IQueryable<T> GetQuery(bool eager = false,
           Expression<Func<T, bool>> filter = null);
        T GetByID(long id);

        void Insert(T entity);

        Task<bool> Delete(long id);

        bool Update(T entityToUpdate);

        Task<T> SaveAsync(T model);

        Task<long> FindIdByBkAsync(
            Expression<Func<T, bool>> filter = null);

        bool IsItNew(DbContext context,
               object entity);
    }
}
