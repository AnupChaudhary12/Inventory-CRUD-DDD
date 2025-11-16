using Inventory.Application.Contracts.PersistenceInterfaces;
using Inventory.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        //string? connectionString = configuration.GetConnectionString("DefaultConnection");

        //services.AddDbContext<InventoryDbContext>(options =>
        //    options.UseSqlite(connectionString ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));
        services.AddDbContext<InventoryDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection")));

        services.AddScoped<IInventoryRepository, InventoryRepository>();

        return services;
    }
}
