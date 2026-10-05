namespace VendorHub.Application.Features.Products.GetProductById;

using System;
using VendorHub.Application.Common.Messaging;

public sealed record GetProductByIdQuery(Guid ProductId) : IQuery<ProductResponse>;
