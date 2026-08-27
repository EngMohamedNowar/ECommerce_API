using ECommerce.API.Handlers;
using ECommerce.API.MiddleWares;

namespace ECommerce.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(typeof(DependencyInjection).Assembly);

        services.AddEndpointsApiExplorer();

        services.AddProblemDetails();
        services.AddSwaggerGen();

        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionMiddleware>();

        return services;
    }
}
