using Dishes.Common.Extensions;
using Dishes.Infrastructure;
using Dishes.MVC.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
  await app.Services.MigrateDatabaseToLatestVersion<DishesDbContext>();
}

await app.ConfigurePipeline();

await app.RunAsync();
