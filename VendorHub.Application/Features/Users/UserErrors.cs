namespace VendorHub.Application.Features.Users;

using VendorHub.Application.Common.Models;

public static class UserErrors
{
    public static readonly Error EmailAlreadyInUse = new(
        "User.EmailAlreadyInUse",
        "The specified email address is already registered.");

    public static readonly Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "Invalid email or password.");
}
