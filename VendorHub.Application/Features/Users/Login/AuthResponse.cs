using System;
using System.Collections.Generic;
using System.Text;

namespace VendorHub.Application.Features.Users.Login
{
    public sealed record AuthResponse(
        string Token,
        Guid UserId,
        string Email,
        string Role,
        string FullName);
}
