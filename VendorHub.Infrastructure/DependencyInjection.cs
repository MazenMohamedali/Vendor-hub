using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VendorHub.Application.Common.Interfaces;
using VendorHub.Domain.Repositories;
using VendorHub.Domain.Services;
using VendorHub.Infrastructure.Authentication;
using VendorHub.Infrastructure.BackGroundJobs;
using VendorHub.Infrastructure.Identity;
using VendorHub.Infrastructure.Persistence;
using VendorHub.Infrastructure.Persistence.Repositories;
using VendorHub.Infrastructure.Services;

namespace VendorHub.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("sqlServerCs");

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString
                , b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Authentication & Identity Services                                        
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddScoped<IIdentityService, IdentityService>();

            // Redis Distributed Cache                                                 
            var redisConnection = configuration.GetConnectionString("RedisConnection") ?? "localhost:6379";
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
                options.InstanceName = "VendorHub_";
            });

            services.AddSingleton<ICacheService, RedisCacheService>();

            // Domain Services
            services.AddScoped<OrderFulfillmentService>();

            // Background Hosted Services
            services.AddHostedService<OrderProcessingBackgroundService>();

            return services;
        }
    }
}