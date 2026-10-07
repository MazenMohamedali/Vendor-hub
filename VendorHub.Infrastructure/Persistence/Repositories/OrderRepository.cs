using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VendorHub.Domain.Common;
using VendorHub.Domain.Entities.Orders;
using VendorHub.Domain.Enums;
using VendorHub.Domain.Repositories;
using VendorHub.Infrastructure.Common;

namespace VendorHub.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(o => o.Items)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.CreatedAtUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Order>> GetPendingOrdersAsync(int batchSize = 20, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(o => o.Items)
                .Where(o => o.Status == OrderStatus.Pending)
                .OrderBy(o => o.CreatedAtUtc)
                .Take(batchSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedList<Order>> GetOrdersAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAtUtc)
                .ToPagedListAsync(page, pageSize, cancellationToken);
        }
    }
}
