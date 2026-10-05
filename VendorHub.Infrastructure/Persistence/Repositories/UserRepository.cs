using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VendorHub.Domain.Entities.Users;
using VendorHub.Domain.Repositories;

namespace VendorHub.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            return await DbSet
                .FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);
        }
    }
}
