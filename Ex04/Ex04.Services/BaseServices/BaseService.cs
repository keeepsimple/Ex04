using Ex04.Common;
using Ex04.Data.Infrastructure;
using Ex04.Models.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ex04.BusinessLayer.BaseServices
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual int Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            _unitOfWork.CoreRepository<T>().Add(entity);
            return _unitOfWork.SaveChanges();
        }

        public virtual async Task<int> AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            _unitOfWork.CoreRepository<T>().Add(entity);
            return await _unitOfWork.SaveChangesAsync();
        }

        public bool Delete(int id)
        {
            var entity = _unitOfWork.CoreRepository<T>().GetById(id);
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            _unitOfWork.CoreRepository<T>().Delete(entity);
            return _unitOfWork.SaveChanges() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = _unitOfWork.CoreRepository<T>().GetById(id);
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            _unitOfWork.CoreRepository<T>().Delete(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public bool Delete(T entity)
        {
            _unitOfWork.CoreRepository<T>().Delete(entity);
            return _unitOfWork.SaveChanges() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _unitOfWork.CoreRepository<T>().Delete(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public virtual IEnumerable<T> GetAll(bool canLoadDeleted = false)
        {
            if (canLoadDeleted)
            {
                return _unitOfWork.CoreRepository<T>().GetQuery().ToList();
            }
            else
            {
                return _unitOfWork.CoreRepository<T>().GetQuery(x => x.IsDeleted == canLoadDeleted).ToList();
            }

        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(bool canLoadDeleted = false)
        {
            if (canLoadDeleted)
            {
                return await _unitOfWork.CoreRepository<T>().GetQuery().ToListAsync();
            }
            else
            {
                return await _unitOfWork.CoreRepository<T>().GetQuery(x => x.IsDeleted == canLoadDeleted).ToListAsync();
            }
        }

        public virtual T GetById(int id)
        {
            return _unitOfWork.CoreRepository<T>().GetById(id);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _unitOfWork.CoreRepository<T>().GetByIdAsync(id);
        }

        public virtual bool Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            _unitOfWork.CoreRepository<T>().Update(entity);
            return _unitOfWork.SaveChanges() > 0;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }
            _unitOfWork.CoreRepository<T>().Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<Paginated<T>> GetAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int pageIndex = 1, int pageSize = 10, bool canLoadDeleted = false)
        {
            var query = _unitOfWork.CoreRepository<T>().GetQuery();
            if (filter != null) query = query.Where(filter);
            if (orderBy != null) query = orderBy(query);
            if (!canLoadDeleted) query = query.Where(x => x.IsDeleted == canLoadDeleted);

            return await Paginated<T>.CreateAsync(query.AsNoTracking(), pageIndex, pageSize);
        }

        public async Task<int> Count(Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool canLoadDeleted = false)
        {
            var query = _unitOfWork.CoreRepository<T>().GetQuery();
            if (filter != null) query = query.Where(filter);
            if (orderBy != null) query = orderBy(query);
            if (!canLoadDeleted) query = query.Where(x => x.IsDeleted == canLoadDeleted);

            return await query.CountAsync();
        }
    }
}