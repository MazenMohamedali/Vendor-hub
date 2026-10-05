using Microsoft.AspNetCore.Identity;
using VendorHub.Application.Common.Interfaces;
using VendorHub.Application.Common.Models;
using VendorHub.Application.Features.Users;
using VendorHub.Application.Features.Users.Login;
using VendorHub.Domain.Repositories;

namespace VendorHub.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUserRepository _userRepository; 

        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IJwtProvider jwtProvider, IUserRepository userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtProvider = jwtProvider;
            _userRepository = userRepository;
        }

        public async Task<Result<AuthResponse>> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return UserErrors.InvalidCredentials;

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
                return UserErrors.InvalidCredentials;

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Customer";

            var domainUser = await _userRepository.GetByIdAsync(user.Id, cancellationToken);
            var fullName = domainUser != null
                ? $"{domainUser.FirstName} {domainUser.LastName}".Trim() 
                : (user.UserName ?? user.Email ?? string.Empty);

            var token = _jwtProvider.GenerateToken(user.Id, user.Email!, role, fullName);

            return Result.Success(new AuthResponse(
                token,
                user.Id,
                user.Email!,
                role,
                fullName));
        }

        public async Task<Result<Guid>> CreateUserAsync(Guid userId, string email, string password, string role, string fullName, CancellationToken cancellationToken = default)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser is not null)
                return UserErrors.EmailAlreadyInUse;
            
            if(!await _roleManager.RoleExistsAsync(role))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = role,
                    NormalizedName = role.ToUpperInvariant()
                });

                if (!roleResult.Succeeded)
                {
                    var errorDesc = roleResult.Errors.FirstOrDefault()?.Description ?? "Failed to create role.";

                    return Result.Failure<Guid>(new Error("Identity.RoleCreationFailed", errorDesc));
                }
            }

            var appUser = new ApplicationUser
            {
                Id = userId,
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(appUser, password);
            if(!createResult.Succeeded)
            {
                var errorDesc = createResult.Errors.FirstOrDefault()?.Description ?? "Failed to create user.";
                return new Error(("Identity.UserCreationFailed"),  errorDesc);
            }

            var roleAssignResult = await _userManager.AddToRoleAsync(appUser, role);
            if (!roleAssignResult.Succeeded)
            {
                var errorDesc = roleAssignResult.Errors.FirstOrDefault()?.Description ?? "Failed to assign role to user.";
                
                return Result.Failure<Guid>(new Error("Identity.RoleAssignmentFailed", errorDesc));
            }

            return Result.Success(appUser.Id);
        }
    }
}
