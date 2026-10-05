using VendorHub.Application.Features.Users.Login;

namespace VendorHub.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<AuthResponse>> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
        
        Task<Result<Guid>> CreateUserAsync(Guid userId, string email, string password, string role, string fullName, CancellationToken cancellationToken = default);
    }
}
