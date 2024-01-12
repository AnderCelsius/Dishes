using Dishes.Common.Configurations;
using Dishes.Common.Extensions;
using Dishes.Infrastructure;
using Dishes.Infrastructure.Data;
using Serilog;

namespace Dishes.API.Extensions;

public static class HostingExtensions
{
  public static void AddServices(this WebApplicationBuilder builder)
  {
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHealthChecks()
      .AddDbContextCheck<DishesDbContext>();

    builder.Services.AddDatabase<DishesDbContext>(builder.Configuration
  .GetRequiredSection("Database").Get<DatabaseConfiguration>());
  }

  public static async Task ConfigurePipeline(this WebApplication app)
  {
    if (!app.Environment.IsProduction())
    {
      Log.Information("Seeding database...");
      await Seeder.EnsureSeedAppData(app);
      Log.Information("Done seeding database. Exiting.");

      app.UseDeveloperExceptionPage();
      app.UseSwagger().UseSwaggerUI();
    }

    app.UseHealthChecks("/health");

    app.UseHttpsRedirection();
  }

}