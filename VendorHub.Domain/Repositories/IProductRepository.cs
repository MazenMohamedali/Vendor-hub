namespace VendorHub.Domain.Repositories;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VendorHub.Domain.Entities.Products;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    void Update(Product product);
    void Remove(Product product);
}
