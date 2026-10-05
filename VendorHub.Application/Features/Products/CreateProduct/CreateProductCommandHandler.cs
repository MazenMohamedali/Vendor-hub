using VendorHub.Application.Common.Models;
using VendorHub.Domain.Entities.Products;
using VendorHub.Domain.Repositories;
using VendorHub.Domain.ValueObjects;

namespace VendorHub.Application.Features.Products.CreateProduct
{
    public sealed class CreateProductCommandHandler
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateProductCommand request,
    CancellationToken cancellationToken)
        {
            var price = new Money(request.Price, request.Currency);

            var product = Product.Create(
                request.Name,
                price,
                request.ImgUrl,
                request.Quantity,
                request.VendorId,
                request.CategoryId);

            await _productRepository.AddAsync(product, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(product.Id);
        }
    }
}
