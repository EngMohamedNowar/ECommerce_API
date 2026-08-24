using ECommerce.API.MiddleWares;
using Microsoft.OpenApi.Models;

namespace ECommerce.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers()
                .AddApplicationPart(typeof(DependencyInjection).Assembly);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ECommerce API",
                    Version = "v1"
                });
            });

            services.AddProblemDetails();

            services.AddExceptionHandler<GlobalExceptionMiddleware>();

            return services;
        }
    }
}
