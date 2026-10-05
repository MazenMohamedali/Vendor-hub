using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VendorHub.Domain.Common;

namespace VendorHub.Infrastructure.Persistence.Repositories
{
    public abstract class GenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<T> DbSet;

        protected GenericRepository(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
        }
        public virtual void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public virtual void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        protected virtual IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        protected virtual IQueryable<T> GetQueryableAsNoTracking(Expression<Func<T, bool>> predicate)
        {
            return DbSet.AsNoTracking().Where(predicate);
        }
    }
}
