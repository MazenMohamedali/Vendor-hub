namespace VendorHub.Application.Features.Orders.GetOrders;

using VendorHub.Application.Common.Messaging;
using VendorHub.Application.Features.Orders.GetOrderById;
using VendorHub.Domain.Common;

public sealed record GetOrdersQuery(int Page = 1, int PageSize = 20) : IQuery<PagedList<OrderResponse>>;
