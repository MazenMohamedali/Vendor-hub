namespace VendorHub.Controllers;

using Microsoft.AspNetCore.Mvc;
using VendorHub.Controllers.Common;
using VendorHub.Application.Features.Products.CreateProduct;
using VendorHub.Application.Features.Products.GetProductById;
using VendorHub.Application.Features.Products.ApproveProduct;

public class ProductsController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure)
            return HandleResult(result);

        return CreatedAtAction(nameof(GetProductById), new { id = result.Value }, result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetProductByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}/approve")]
    public async Task<IActionResult> ApproveProduct(Guid id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new ApproveProductCommand(id), cancellationToken);
        return HandleResult(result);
    }
}
