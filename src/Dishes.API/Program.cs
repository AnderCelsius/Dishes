using Dishes.API.Extensions;
using Dishes.Common.Extensions;
using Dishes.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    await app.Services.MigrateDatabaseToLatestVersion<DishesDbContext>();
}

await app.ConfigurePipeline();

await app.RunAsync();

