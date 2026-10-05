namespace VendorHub.Controllers;

using Microsoft.AspNetCore.Mvc;
using VendorHub.Controllers.Common;
using VendorHub.Application.Features.Orders.CreateOrder;
using VendorHub.Application.Features.Orders.GetOrderById;
using VendorHub.Application.Features.Orders.CancelOrder;

public class OrdersController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure)
            return HandleResult(result);

        return CreatedAtAction(nameof(GetOrderById), new { id = result.Value }, result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetOrderByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> CancelOrder(Guid id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new CancelOrderCommand(id), cancellationToken);
        return HandleResult(result);
    }
}
