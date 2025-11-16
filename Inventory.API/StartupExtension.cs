
using Delta;
using Inventory.API.Middleware;
using Inventory.Application;
using Inventory.Persistence;
using Microsoft.Extensions.Configuration;
namespace Inventory.API;

public static class StartupExtension
{
    public static WebApplicationBuilder BuilderService(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        // Add services to the container
        builder.Services.AddControllers();
        // Library Service Registration
        builder.Services.AddApplicationServices();
        builder.Services.AddPersistenceServices(builder.Configuration);

        builder.Services.AddSwaggerGen();

        builder.Services.AddOpenApi();
        return builder;
    }

    public static WebApplication AppPipeline(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
