using Microsoft.EntityFrameworkCore;
using VendorHub.Domain.Entities.Products;
using VendorHub.Domain.Repositories;

namespace VendorHub.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Category>> GetAllActiveAsync(CancellationToken
  cancellationToken = default)
        {
            return await DbSet
                .Where(c => c.IsActive)
                .ToListAsync(cancellationToken);
        }
    }
}
