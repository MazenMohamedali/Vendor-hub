using Microsoft.AspNetCore.Mvc;
using VendorHub.Application.Features.Users.Login;
using VendorHub.Application.Features.Users.RegisterCustomer;
using VendorHub.Application.Features.Users.RegisterVendor;
using VendorHub.Controllers.Common;

namespace VendorHub.API.Controllers
{
    public class AuthController : BaseApiController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken) => 
            HandleResult(await Sender.Send(command, cancellationToken));

        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerCommand command, CancellationToken cancellationToken) => 
            HandleResult(await Sender.Send(command, cancellationToken));

        [HttpPost("register-vendor")]
        public async Task<IActionResult> RegisterVendor([FromBody] RegisterVendorCommand command, CancellationToken cancellation) => 
            HandleResult(await Sender.Send(command, cancellation));
    }
}
