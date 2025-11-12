using System.Reflection;
using Inventory.Application.Contracts.Services;
using Inventory.Application.Contracts.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
        );
        services.AddScoped<IErrorHandlingService, ErrorHandlingService>();
        return services;
    }

}
