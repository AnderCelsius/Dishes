using Dishes.Common.Models;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Dishes.API.Extensions;

public static class ODataConventionModelBuilderExtensions
{
    public static IEdmModel BuildODataConventionModel()
    {
        var odataBuilder = new ODataConventionModelBuilder();
        odataBuilder.EntitySet<DishListDTO>("Dishes");
        odataBuilder.EntitySet<IngredientDTO>("Ingredients");
        IEdmModel edmModel = odataBuilder.GetEdmModel();

        return edmModel;
    }
}
