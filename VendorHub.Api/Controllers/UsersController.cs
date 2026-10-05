namespace VendorHub.Controllers;

using Microsoft.AspNetCore.Mvc;
using VendorHub.Controllers.Common;
using VendorHub.Application.Features.Users.RegisterCustomer;
using VendorHub.Application.Features.Users.RegisterVendor;

public class UsersController : BaseApiController
{
    [HttpPost("register-customer")]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("register-vendor")]
    public async Task<IActionResult> RegisterVendor([FromBody] RegisterVendorCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return HandleResult(result);
    }
}
