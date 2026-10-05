using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VendorHub.Domain.Entities.Products;
using VendorHub.Domain.Enums;
using VendorHub.Domain.Repositories;

namespace VendorHub.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Product>> GetByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(p => p.VendorId == vendorId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> GetAllActiveAsync(CancellationToken
  cancellationToken = default)
        {
            return await DbSet
                .Where(p => p.Status == ProductStatus.Reviewed && p.Quantity > 0)
                .ToListAsync(cancellationToken);
        }
    }
}
