using VendorHub.Application.Common.Messaging;

namespace VendorHub.Application.Features.Users.Login
{
    public sealed record LoginCommand(string Email, string Password) : ICommand<AuthResponse>;
}
