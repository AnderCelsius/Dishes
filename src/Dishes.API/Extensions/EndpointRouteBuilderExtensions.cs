using Dishes.API.Endpoints;
using Dishes.Common.Models;

namespace Dishes.API.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterDishesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        // Define the main group for dishes
        var dishesOdataEndpoints = endpointRouteBuilder.MapGroup("/odata/dishes");
        var dishesEndpoints = endpointRouteBuilder.MapGroup("/dishes");

        // Define the sub-group for dish operations with a GUID identifier
        var dishWithGuidIdEndpoints = dishesEndpoints.MapGroup("/{dishId:guid}");

        // Register the endpoints
        dishesOdataEndpoints.MapGet("", DishesEndpoints.GetDishesAsync)
            .WithName("GetDishes")
            .Produces<PagedResponse<DishListDTO>>(StatusCodes.Status200OK);

        dishWithGuidIdEndpoints.MapGet("", DishesEndpoints.GetDishByIdAsync)
            .WithName("GetDish")
            .WithOpenApi()
            .WithSummary("Get a dish by providing an id.")
            .WithDescription("Dishes are identified by a URI containing a dish identifier. This identifier is a GUID. You can get one specific dish via this endpoint by providing the identifier.");

        dishWithGuidIdEndpoints.MapDelete("", DishesEndpoints.DeleteDishAsync);

        dishesEndpoints.MapPost("", DishesEndpoints.CreateDishAsync)
            .Accepts<Features.Dishes.Create.Request>("application/json");

        //dishWithGuidIdEndpointsAndLockFilters.MapPut("", DishesEndpoints.UpdateDishAsync);

        var dishIngredientsEndpoints = endpointRouteBuilder.MapGroup("/odata/dishes/{dishId:guid}/ingredients");

        dishIngredientsEndpoints.MapGet("", DishesEndpoints.GetDishIngredientsAsync);

    }


    public static void RegisterIngredientsEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        // Define the main group for ingredients
        var ingredientsODataEndpoints = endpointRouteBuilder.MapGroup("/odata/ingredients");

        var ingredientsEndpoints = endpointRouteBuilder.MapGroup("/ingredients");
        var ingredientWithGuidIdEndpoints = ingredientsEndpoints.MapGroup("/{ingredientId:guid}");

        ingredientsEndpoints.MapPost("", IngredientsEndpoints.CreateIngredientAsync)
            .WithName("CreateIngredient")
            .Accepts<Features.Ingredients.Create.Request>("application/json");

        ingredientsODataEndpoints.MapGet("", IngredientsEndpoints.GetIngredientsAsync)
            .WithName("GetIngredients")
            .Produces<PagedResponse<IngredientDTO>>(StatusCodes.Status200OK);

        ingredientWithGuidIdEndpoints.MapGet("", IngredientsEndpoints.GetIngredientByIdAsync)
            .WithName("GetIngredient")
            .WithOpenApi()
            .WithSummary("Get an ingredient by providing an id.")
            .WithDescription("Ingredients are identified by a URI containing an ingredient identifier. This identifier is a GUID. You can get one specific ingredient via this endpoint by providing the identifier.");
    }
}
