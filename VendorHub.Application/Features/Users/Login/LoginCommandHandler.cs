using System;
using System.Threading;
using System.Threading.Tasks;
using VendorHub.Application.Common.Interfaces;
using VendorHub.Application.Common.Messaging;
using VendorHub.Application.Common.Models;

namespace VendorHub.Application.Features.Users.Login
{
    public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, AuthResponse>
    {
        private readonly IIdentityService _idenetityService;
        public LoginCommandHandler(IIdentityService identityService)
        {
            _idenetityService = identityService;
        }

        public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken) 
        {
            return await _idenetityService.AuthenticateAsync(request.Email, request.Password, cancellationToken);
        }
    }
}
