namespace VendorHub.Application.Features.Orders.CreateOrder;

using System;
using System.Threading;
using System.Threading.Tasks;
using VendorHub.Application.Common.Messaging;
using VendorHub.Application.Common.Models;
using VendorHub.Domain.Entities.Orders;
using VendorHub.Domain.Repositories;
using VendorHub.Domain.ValueObjects;

public sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var address = new Address(
            request.Street,
            request.City,
            request.State,
            request.Country,
            request.PostalCode);

        var currency = string.IsNullOrWhiteSpace(request.Currency) ? "EGP" : request.Currency.Trim();

        var order = Order.Create(
            request.CustomerId,
            address,
            request.PhoneNumber,
            currency);

        foreach (var item in request.Items)
        {
            var unitPrice = new Money(item.UnitPrice, item.Currency);
            order.AddItem(item.ProductId, item.VendorId, unitPrice, item.Quantity);
        }

        await _orderRepository.AddAsync(order, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(order.Id);
    }
}
