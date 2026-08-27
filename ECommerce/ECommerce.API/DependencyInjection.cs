using ECommerce.API.Handlers;
using ECommerce.API.MiddleWares;
using Microsoft.OpenApi.Models;

namespace ECommerce.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(typeof(DependencyInjection).Assembly);

        services.AddEndpointsApiExplorer();

        services.AddProblemDetails();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ECommerce API",
                Version = "v1",
                Description = "REST API for an e-commerce platform built with Clean Architecture."
            });
        });

        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionMiddleware>();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }
}