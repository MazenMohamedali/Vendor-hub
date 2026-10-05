namespace VendorHub.Application.Features.Products.ApproveProduct;

using System.Threading;
using System.Threading.Tasks;
using VendorHub.Application.Common.Messaging;
using VendorHub.Application.Common.Models;
using VendorHub.Domain.Repositories;

public sealed class ApproveProductCommandHandler : ICommandHandler<ApproveProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ApproveProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            return Result.Failure(ProductErrors.NotFound(request.ProductId));
        }

        product.Approve();

        _productRepository.Update(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
