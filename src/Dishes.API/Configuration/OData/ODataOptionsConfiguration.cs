using Dishes.API.Extensions;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Options;

namespace Dishes.API.Configuration.Problems;

public class ODataOptionsConfiguration : IConfigureOptions<ODataOptions>
{
    public void Configure(ODataOptions options)
    {
        var edmModel = ODataConventionModelBuilderExtensions.BuildODataConventionModel();

        options
            .AddRouteComponents("odata", edmModel)
                .SetMaxTop(100)
                .Select()
                .Filter()
                .OrderBy()
                .SkipToken()
                .Expand()
                .EnableQueryFeatures(100);
    }
}
