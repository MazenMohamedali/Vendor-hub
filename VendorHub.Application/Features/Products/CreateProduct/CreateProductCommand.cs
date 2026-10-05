using VendorHub.Application.Common.Messaging;

namespace VendorHub.Application.Features.Products.CreateProduct
{
    public sealed record CreateProductCommand(
        string Name,
        decimal Price,
        string Currency,
        string ImgUrl,
        int Quantity,
        Guid VendorId,
        Guid CategoryId) : ICommand<Guid>;
}
