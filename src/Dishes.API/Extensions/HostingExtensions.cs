using AutoMapper;
using Dishes.Common.Configurations;
using Dishes.Common.Extensions;
using Dishes.Common.MapProfiles;
using Dishes.Common.Models;
using Dishes.Core.Entities;
using Dishes.Infrastructure;
using Dishes.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Serilog;

namespace Dishes.API.Extensions;

public static class HostingExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHealthChecks()
          .AddDbContextCheck<DishesDbContext>();

        builder.Services.AddAutoMapper(typeof(DishProfile).Assembly);


        var modelBuilder = new ODataConventionModelBuilder();
        modelBuilder.EntitySet<DishDto>("Dishes");

        builder.Services.AddControllers()
            .AddOData(options => options.Select().Filter().OrderBy().SetMaxTop(100).Expand()
            .AddRouteComponents(
            "odata", modelBuilder.GetEdmModel()));

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

        app.UseODataBatching();

        app.UseHealthChecks("/health");

        app.MapGet("/dishes", async Task<Ok<IEnumerable<DishDto>>> (DishesDbContext dishesDbContext, IMapper mapper) =>
        {
            var dishes = await dishesDbContext.Dishes.ToListAsync();
            return TypedResults.Ok(mapper.Map<IEnumerable<DishDto>>(dishes));
        });

        app.MapGet("/dishes/{dishId}", async Task<Results<Ok<DishDto>, NotFound>> (Guid dishId, DishesDbContext dishesDbContext, IMapper mapper) =>
        {
            var dish = await dishesDbContext.Dishes.Where(d => d.Id == dishId).FirstOrDefaultAsync();

            return dish == null ? TypedResults.NotFound() : TypedResults.Ok(mapper.Map<DishDto>(dish));
        });

        app.MapGet("/odata/dishes/", async (HttpContext httpContext, DishesDbContext dishesDbContext, IMapper mapper) =>
        {
            var options = new ODataQueryOptions<Dish>(new ODataQueryContext(GetEdmModel(), typeof(Dish), null), httpContext.Request);
            var query = options.ApplyTo(dishesDbContext.Dishes.AsQueryable());
            var results = await query.Cast<Dish>().ToListAsync();
            return Results.Ok(mapper.Map<IEnumerable<DishDto>>(results));
        }).Produces<IEnumerable<DishDto>>();



        app.UseHttpsRedirection();

        app.MapDefaultEndpoints();
    }


    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<Dish>("Dishes");
        return builder.GetEdmModel();
    }
}