namespace VendorHub.Application.Features.Users.RegisterCustomer;

using System;
using VendorHub.Application.Common.Messaging;

public sealed record RegisterCustomerCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<Guid>;
