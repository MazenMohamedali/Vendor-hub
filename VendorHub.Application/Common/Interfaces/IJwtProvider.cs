using System;
using System.Collections.Generic;
using System.Text;

namespace VendorHub.Application.Common.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(Guid userId, string email, string role, string fullName);
    }
}
