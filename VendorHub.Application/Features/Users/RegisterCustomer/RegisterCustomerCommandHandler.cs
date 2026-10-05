namespace VendorHub.Application.Features.Users.RegisterCustomer;

using System;
using System.Threading;
using System.Threading.Tasks;
using VendorHub.Application.Common.Messaging;
using VendorHub.Application.Common.Models;
using VendorHub.Domain.Entities.Users;
using VendorHub.Domain.Repositories;

public sealed class RegisterCustomerCommandHandler : ICommandHandler<RegisterCustomerCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCustomerCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        RegisterCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (existingUser is not null)
        {
            return Result.Failure<Guid>(UserErrors.EmailAlreadyInUse);
        }

        var customer = Customer.Create(
            request.FirstName,
            request.LastName,
            request.Email);

        await _userRepository.AddAsync(customer, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(customer.Id);
    }
}
