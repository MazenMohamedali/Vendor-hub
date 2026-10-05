namespace VendorHub.Application.Features.Users.RegisterVendor;

using System;
using System.Threading;
using System.Threading.Tasks;
using VendorHub.Application.Common.Interfaces;
using VendorHub.Application.Common.Messaging;
using VendorHub.Application.Common.Models;
using VendorHub.Domain.Entities.Users;
using VendorHub.Domain.Repositories;

public sealed class RegisterVendorCommandHandler : ICommandHandler<RegisterVendorCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterVendorCommandHandler(
        IUserRepository userRepository,
        IIdentityService identityService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _identityService = identityService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        RegisterVendorCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (existingUser is not null)
            return UserErrors.EmailAlreadyInUse;

        var vendor = Vendor.Create(
            request.FirstName,
            request.LastName,
            request.Email,
            request.StoreName);

        var identityResult = await _identityService.CreateUserAsync(
            vendor.Id,
            request.Email,
            request.Password,
            "Vendor",
            $"{request.FirstName} {request.LastName}",
            cancellationToken);

        await _userRepository.AddAsync(vendor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(vendor.Id);
    }   
}