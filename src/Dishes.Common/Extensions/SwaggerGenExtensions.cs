using Dishes.Common.Configurations.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dishes.Common.Extensions;

public static class SwaggerGenExtensions
{
    /// <summary>
    /// Sets up Swagger schema generation with additional settings.
    /// </summary>
    /// <param name="services">IServiceCollection.</param>
    /// <param name="dtoModelNamespace">
    /// for all types and namespaces nested in
    /// "dtoModelNamespace", will replace "+" and "." in the type name
    /// with "_". E.g. Features.NS.GetAll+Response => Features.Books_GetAll_Response.
    /// </param>
    /// <returns>services.</returns>
    public static IServiceCollection AddSwaggerGenWithExtraSetup(
      this IServiceCollection services,
      IConfiguration config,
      string? dtoModelNamespace)
    {
        services.AddSwaggerGen(opt =>
        {
            // Set up to concatenate namespaces
            opt.CustomSchemaIds(x => x.FullName?.ConcatenateDtoNamespaces(dtoModelNamespace).Replace("+", "."));
            opt.SupportNonNullableReferenceTypes();
            opt.UseAllOfToExtendReferenceSchemas();
            opt.SchemaFilter<MakeNotNullableRequiredSchemaFilter>();

            opt.OperationFilter<ODataOperationFilter>();

            opt.TryIncludeXmlComments();

            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer",
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer", },
                    },
                    new List<string>()
                },
            });
        });

        return services;
    }

    private static void TryIncludeXmlComments(this SwaggerGenOptions options)
    {
        try
        {
            var baseDir = System.AppContext.BaseDirectory;
            var apiName = baseDir.Split(Path.DirectorySeparatorChar)
              .First(s => s.EndsWith(".API"));
            var filePath = Path.Combine(System.AppContext.BaseDirectory, $"{apiName}.xml");
            options.IncludeXmlComments(filePath);
        }
        catch
        {
            // Failed to include XML Comments, skipping
        }
    }

    private static string ConcatenateDtoNamespaces(this string typeFullName, string? target)
    {
        if (target is null)
        {
            return typeFullName;
        }

        var index = typeFullName.IndexOf($".{target}.", StringComparison.InvariantCultureIgnoreCase);
        if (index == -1)
        {
            return typeFullName;
        }

        var dtoRelativeTypeName = typeFullName[(index + $".{target}.".Length)..];
        var dtoRenamed = dtoRelativeTypeName
          .Replace("+", "_") // separate nested types by _ instead of +
          .Replace(".", "_"); // separate namespaces by _ instead of .
        Console.WriteLine($"Renaming {typeFullName} --> {typeFullName.Replace(dtoRelativeTypeName, dtoRenamed)}");
        return typeFullName.Replace(dtoRelativeTypeName, dtoRenamed);
    }

    /// <summary>
    /// Finds all schema properties that are not nullable, and marks them as required.
    /// This removes the "|undefined" from the generated typescript client.
    /// </summary>
    public class MakeNotNullableRequiredSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null)
            {
                return;
            }

            var notNullableProperties = schema
              .Properties
              .Where(x => !x.Value.Nullable && !schema.Required.Contains(x.Key))
              .ToList();

            foreach (var property in notNullableProperties)
            {
                schema.Required.Add(property.Key);
            }
        }
    }
}

