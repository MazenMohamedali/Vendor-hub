namespace VendorHub.Application.Features.Users.RegisterVendor;

using System;
using VendorHub.Application.Common.Messaging;

public sealed record RegisterVendorCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string StoreName) : ICommand<Guid>;
