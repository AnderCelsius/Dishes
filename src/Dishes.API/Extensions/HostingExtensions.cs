using Dishes.API.Configuration.Middleware;
using Dishes.API.Dispatchers;
using Dishes.API.Services;
using Dishes.Common.Configurations;
using Dishes.Common.Extensions;
using Dishes.Common.MapProfiles;
using Dishes.Core.Contracts;
using Dishes.Infrastructure;
using Dishes.Infrastructure.Data;
using Dishes.Infrastructure.Repositories;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Serilog;
using System.Reflection;

namespace Dishes.API.Extensions;

public static class HostingExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.AddFluentValidationExtension();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddConfigurationOptions();

        Hellang.Middleware.ProblemDetails.ProblemDetailsExtensions.AddProblemDetails(builder.Services);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGenWithExtraSetup(builder.Configuration, nameof(Features));

        builder.Services.AddHttpClient("WebhookService", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7159/api/webhook/");
        });

        builder.Services.AddHealthChecks()
          .AddDbContextCheck<DishesDbContext>();

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        builder.Services.AddAutoMapper(typeof(DishProfile).Assembly);

        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // 1. Build EDM Model
        var edmModel = ODataConventionModelBuilderExtensions.BuildODataConventionModel();

        // 2. Register IEdmModel as a Singleton
        builder.Services.AddSingleton<IEdmModel>(edmModel);

        builder.Services.AddScoped<IWebhookService, WebhookService>();

        builder.Services.AddScoped<IWebhookEventDispatcher, WebhookEventDispatcher>();

        builder.Services
            .AddControllers()
            .AddOData()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });

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

        app.UseRouting();

        app.UseODataRouteDebug();

        app.UseODataBatching();

        app.UseHealthChecks("/health");

        app.RegisterDishesEndpoints();
        app.RegisterIngredientsEndpoints();

        app.UseHttpsRedirection();

        app.MapDefaultEndpoints();
    }
}