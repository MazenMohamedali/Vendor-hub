namespace VendorHub.Application.Features.Products.ApproveProduct;

using System;
using VendorHub.Application.Common.Messaging;

public sealed record ApproveProductCommand(Guid ProductId) : ICommand;
