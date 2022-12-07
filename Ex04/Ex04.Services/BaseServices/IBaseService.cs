using Ex04.Common;
using System.Linq.Expressions;

namespace Ex04.BusinessLayer.BaseServices
{
    public interface IBaseService<T>
    {
        int Add(T entity);

        Task<int> AddAsync(T entity);

        bool Update(T entity);

        Task<int> Count(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool canLoadDeleted = false);

        Task<bool> UpdateAsync(T entity);

        bool Delete(int id);

        Task<bool> DeleteAsync(int id);

        bool Delete(T entity);

        Task<bool> DeleteAsync(T entity);

        T GetById(int id);

        Task<T> GetByIdAsync(int id);

        IEnumerable<T> GetAll(bool canLoadDeleted = false);

        Task<IEnumerable<T>> GetAllAsync(bool canLoadDeleted = false);

        Task<Paginated<T>> GetAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int pageIndex = 1, int pageSize = 10, bool canLoadDeleted = false);
    }
}