using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace Dishes.API.Configuration.OData;

public static class ODataQueryOptionsFactory
{
    public static ODataQueryOptions<TDTO> CreateQueryOptions<TDTO>(HttpRequest request)
    {
        // Retrieve the EDM model from the request's services
        var edmModel = request.HttpContext.RequestServices.GetRequiredService<IEdmModel>();

        // Optionally, retrieve the OData path from the request if needed
        var odataPath = new ODataPath(); // Customize as per your routing configuration

        // Create the OData query context
        var queryContext = new ODataQueryContext(edmModel, typeof(TDTO), odataPath);

        // Construct the OData query options
        return new ODataQueryOptions<TDTO>(queryContext, request);
    }
}

