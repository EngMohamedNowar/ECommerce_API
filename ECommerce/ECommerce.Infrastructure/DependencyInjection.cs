using ECommerce.Domain.Identity;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence.Basket;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Interceptors;
using ECommerce.Infrastructure.Persistence.Queries;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Security;
using ECommerce.UseCases.Auth.Contracts;
using ECommerce.UseCases.Basket.Contracts;
using ECommerce.UseCases.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ECommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntityInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();

        services.AddDbContext<StoreDbContext>((serviceProvider, options) =>
        {
            var auditableInterceptor = serviceProvider.GetRequiredService<AuditableEntityInterceptor>();
            var softDeleteInterceptor = serviceProvider.GetRequiredService<SoftDeleteInterceptor>();

            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                })
                .AddInterceptors(auditableInterceptor, softDeleteInterceptor);
        });

        services.AddIdentityCore<AppUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<StoreDbContext>();

        services.Configure<AuthOptions>(configuration.GetSection(AuthOptions.SectionName));

        services.AddScoped<IAuthTokenService, JwtProvider>();

        services.AddScoped<IProductQueryService, ProductQueryService>();
        services.AddScoped<IBrandQueryService, BrandQueryService>();
        services.AddScoped<ITypeQueryService, TypeQueryService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
            {
                EndPoints = { configuration.GetConnectionString("Redis") ?? "localhost:6379" },
                AbortOnConnectFail = true,
                ConnectTimeout = 1500,
                SyncTimeout = 1500,
                ConnectRetry = 0
            };
        });

        services.AddScoped<IBasketRepository, RedisBasketRepository>();

        services.AddScoped<IDataSeeder, AppIdentitySeeder>();
        services.AddScoped<IDataSeeder, ProductBrandSeeder>();
        services.AddScoped<IDataSeeder, ProductTypeSeeder>();
        services.AddScoped<IDataSeeder, ProductSeeder>();
        services.AddScoped<IDataSeeder, DeliveryMethodSeeder>();
        services.AddScoped<DatabaseSeeder>();

        return services;
    }
}