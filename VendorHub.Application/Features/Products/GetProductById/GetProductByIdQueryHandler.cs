namespace VendorHub.Application.Features.Products.GetProductById;

using System.Threading;
using System.Threading.Tasks;
using VendorHub.Application.Common.Messaging;
using VendorHub.Application.Common.Models;
using VendorHub.Domain.Repositories;

public sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductResponse>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<ProductResponse>(ProductErrors.NotFound(request.ProductId));
        }

        var response = new ProductResponse(
            product.Id,
            product.Name,
            product.Price.Amount,
            product.Price.Currency,
            product.ImgUrl,
            product.Quantity,
            product.Status.ToString(),
            product.VendorId,
            product.CategoryId,
            product.ViewersCount);

        return Result.Success(response);
    }
}
