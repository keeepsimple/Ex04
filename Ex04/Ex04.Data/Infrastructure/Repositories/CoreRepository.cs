using Ex04.Models.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ex04.Data.Infrastructure.Repositories
{
    public class CoreRepository<Entity> : ICoreRepository<Entity> where Entity : BaseEntity
    {
        protected readonly NewsContext _context;
        private readonly DbSet<Entity> DbSet;

        public CoreRepository(NewsContext context)
        {
            _context = context ?? throw new ArgumentNullException("context");
            DbSet = context.Set<Entity>();
        }

        public void Add(Entity entity)
        {
            DbSet.Add(entity);
        }

        public async Task<Entity> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual Entity GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual IQueryable<Entity> GetQuery()
        {
            return DbSet;
        }

        public IQueryable<Entity> GetQuery(Expression<Func<Entity, bool>> where)
        {
            return DbSet.Where(where);
        }

        public void Update(Entity entity)
        {
            //_dbSet.AddOrUpdate(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Entity entity, bool isHardDelete = false)
        {
            if (isHardDelete)
            {
                DbSet.Remove(entity);
            }
            else
            {
                entity.IsDeleted = true;
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Delete(IEnumerable<Entity> entities, bool isHardDelete = false)
        {
            if (isHardDelete)
            {
                DbSet.RemoveRange(entities);
            }
            else
            {
                foreach (var entity in entities)
                {
                    entity.IsDeleted = true;
                }
            }
        }
    }
}
