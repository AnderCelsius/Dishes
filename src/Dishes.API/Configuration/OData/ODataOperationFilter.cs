using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dishes.API.Configuration.OData;

public class ODataOperationFilter : IOperationFilter
{
    private readonly List<OpenApiParameter> _odataParameters = new List<OpenApiParameter>
    {
        new OpenApiParameter
        {
            Name = "$filter",
            In = ParameterLocation.Query,
            Description = "Filter the results using OData syntax",
            Required = false,
            Schema = new OpenApiSchema { Type = "string" }
        },
        new OpenApiParameter
        {
            Name = "$orderby",
            In = ParameterLocation.Query,
            Description = "Order the results using OData syntax",
            Required = false,
            Schema = new OpenApiSchema { Type = "string" }
        },
        new OpenApiParameter
        {
            Name = "$top",
            In = ParameterLocation.Query,
            Description = "Limit the number of results returned",
            Required = false,
            Schema = new OpenApiSchema { Type = "integer", Format = "int32" }
        },
        new OpenApiParameter
        {
            Name = "$skip",
            In = ParameterLocation.Query,
            Description = "Skip a number of results",
            Required = false,
            Schema = new OpenApiSchema { Type = "integer", Format = "int32" }
        },
        new OpenApiParameter
        {
            Name = "$count",
            In = ParameterLocation.Query,
            Description = "Include count of total items",
            Required = false,
            Schema = new OpenApiSchema { Type = "boolean" }
        },
        new OpenApiParameter
        {
            Name = "$expand",
            In = ParameterLocation.Query,
            Description = "Expand related entities.",
            Required = false,
            Schema = new OpenApiSchema { Type = "string" }
        }
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var httpMethod = context.ApiDescription.HttpMethod;
        var relativePath = context.ApiDescription.RelativePath;

        if (!(string.Equals(httpMethod, "GET", StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        if (!string.IsNullOrEmpty(relativePath) && !relativePath.Contains("odata"))
        {
            return;
        }

        operation.Parameters ??= new List<OpenApiParameter>();

        foreach (var odataParam in _odataParameters)
        {
            if (!operation.Parameters.Any(p => p.Name.Equals(odataParam.Name, StringComparison.OrdinalIgnoreCase)
                && p.In == odataParam.In))
            {
                operation.Parameters.Add(odataParam);
            }
        }
    }
}
