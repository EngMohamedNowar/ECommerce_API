using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Interceptors;
using ECommerce.Infrastructure.Persistence.Queries;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.Infrastructure.Repositories;
using ECommerce.UseCases.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddScoped<IProductQueryService, ProductQueryService>();
        services.AddScoped<IBrandQueryService, BrandQueryService>();
        services.AddScoped<ITypeQueryService, TypeQueryService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IDataSeeder, ProductBrandSeeder>();
        services.AddScoped<IDataSeeder, ProductTypeSeeder>();
        services.AddScoped<IDataSeeder, ProductSeeder>();
        services.AddScoped<DatabaseSeeder>();

        return services;
    }
}