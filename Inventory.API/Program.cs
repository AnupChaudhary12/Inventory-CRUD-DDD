using Inventory.API;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder = builder.BuilderService();

WebApplication app = builder.Build();

app = app.AppPipeline();

await app.RunAsync();
