namespace VendorHub.Application.Features.Orders.GetOrders;

using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VendorHub.Application.Common.Messaging;
using VendorHub.Application.Common.Models;
using VendorHub.Application.Features.Orders.GetOrderById;
using VendorHub.Domain.Common;
using VendorHub.Domain.Repositories;

public sealed class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, PagedList<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<GetOrdersQueryHandler> _logger;

    public GetOrdersQueryHandler(
        IOrderRepository orderRepository,
        ILogger<GetOrdersQueryHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Result<PagedList<OrderResponse>>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving paginated orders. Page: {Page}, PageSize: {PageSize}", request.Page, request.PageSize);

        var pagedOrders = await _orderRepository.GetOrdersAsync(request.Page, request.PageSize, cancellationToken);

        var orderResponses = pagedOrders.Items.Select(order => new OrderResponse(
            order.Id,
            order.CustomerId,
            order.PhoneNumber,
            order.DeliveryAddress != null
                ? $"{order.DeliveryAddress.Street}, {order.DeliveryAddress.City}, {order.DeliveryAddress.Country}"
                : string.Empty,
            order.Status.ToString(),
            order.TotalPrice.Amount,
            order.TotalPrice.Currency,
            order.Items.Select(i => new OrderItemResponse(
                i.Id,
                i.ProductId,
                i.VendorId,
                i.UnitPrice.Amount,
                i.UnitPrice.Currency,
                i.Quantity,
                i.Status.ToString())).ToList()
        )).ToList();

        var pagedResult = new PagedList<OrderResponse>(
            orderResponses,
            pagedOrders.Page,
            pagedOrders.PageSize,
            pagedOrders.TotalCount);

        return Result.Success(pagedResult);
    }
}
