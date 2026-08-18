using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Persistence.Interceptors;
using ECommerce.Infrastructure.Persistence.Queries;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.UseCases.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure
{
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

                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(auditableInterceptor, softDeleteInterceptor);
            });

            services.AddScoped<IProductQueryService, ProductQueryService>();
            services.AddScoped<IDataSeeder, ProductBrandSeeder>();
            services.AddScoped<IDataSeeder, ProductTypeSeeder>();
            services.AddScoped<DatabaseSeeder>();


            return services;
        }
    }
}