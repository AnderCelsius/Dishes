using Dishes.Common.Authorization;
using Dishes.Common.Configurations;
using Dishes.Common.Extensions;
using Dishes.Common.MapProfiles;
using Dishes.Infrastructure;
using Dishes.Infrastructure.Data;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Dishes.MVC.API.Extensions;

public static class HostingExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            opts.JsonSerializerOptions.DefaultIgnoreCondition =
                JsonIgnoreCondition.WhenWritingNull;
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Dishes API",
                Version = "v1",
                Description = "An API for managing dishes"
            });

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddAutoMapper(typeof(DishProfile).Assembly);

        builder.Services.AddProblemDetails();

        builder.Services.AddHealthChecks()
            .AddDbContextCheck<DishesDbContext>();

        builder.Services.AddDatabase<DishesDbContext>(builder.Configuration
            .GetRequiredSection("Database").Get<DatabaseConfiguration>());

        builder.Services.AddAuthentication().AddJwtBearer();

        builder.Services.AddAuthorization(authorizationOptions =>
        {
            authorizationOptions.AddPolicy(Policies.RequireAdminFromNigeria,
                AuthorizationPolicies.RequireAdminFromNigeria());
        });
    }

    public static async Task ConfigurePipeline(this WebApplication app)
    {
        if (!app.Environment.IsProduction())
        {
            Log.Information("Seeding database...");
            await Seeder.EnsureSeedAppData(app);
            Log.Information("Done seeding database. Exiting.");

            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger().UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dishes API V1");
            c.RoutePrefix = string.Empty; // To serve the Swagger UI at the app's root
        });

        app.UseHealthChecks("/health");

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}