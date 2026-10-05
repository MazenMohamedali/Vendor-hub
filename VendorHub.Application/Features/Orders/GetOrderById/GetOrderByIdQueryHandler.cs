namespace VendorHub.Application.Features.Orders.GetOrderById;

using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VendorHub.Application.Common.Interfaces;
using VendorHub.Application.Common.Messaging;
using VendorHub.Application.Common.Models;
using VendorHub.Domain.Entities.Orders;
using VendorHub.Domain.Repositories;

public sealed class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetOrderByIdQueryHandler> _logger;

    public GetOrderByIdQueryHandler(
        IOrderRepository orderRepository,
        ICacheService cacheService,
        ILogger<GetOrderByIdQueryHandler> logger)
    {
        _orderRepository = orderRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<Result<OrderResponse>> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"orders:{request.OrderId}";

        // 1. Check Redis Cache                                                        
        var cachedOrder = await _cacheService.GetAsync<OrderResponse>(cacheKey, cancellationToken);
        if (cachedOrder is not null)
        {
            _logger.LogInformation("Cache HIT for order {OrderId}", request.OrderId);
            return Result.Success(cachedOrder);
        }

        // 2. Cache Miss: Fetch from SQL Server                                        
        _logger.LogInformation("Cache MISS for order {OrderId}. Fetching from database. ", request.OrderId);                                                                     

        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure<OrderResponse>(OrderErrors.NotFound(request.OrderId));
        }

        var items = order.Items.Select(i => new OrderItemResponse(
            i.Id,
            i.ProductId,
            i.VendorId,
            i.UnitPrice.Amount,
            i.UnitPrice.Currency,
            i.Quantity,
            i.Status.ToString())).ToList();

        var response = new OrderResponse(
            order.Id,
            order.CustomerId,
            order.PhoneNumber,
            order.DeliveryAddress.ToString(),
            order.Status.ToString(),
            order.TotalPrice.Amount,
            order.TotalPrice.Currency,
            items);

        // 3. Save to Redis for next time                                              
        await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);

        return Result.Success(response);
    }
}