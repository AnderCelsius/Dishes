using AutoMapper;
using Dishes.API.Configuration.OData;
using Dishes.API.Extensions;
using Dishes.API.Features.Ingredients;
using Dishes.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;


namespace Dishes.API.Endpoints;

public static class IngredientsEndpoints
{
    [EnableQuery]
    public static async Task<Ok<PagedResponse<IngredientDTO>>> GetIngredientsAsync(
        [FromServices] IMediator mediator,
        [FromServices] IMapper mapper,
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        var queryOptions = ODataQueryOptionsFactory.CreateQueryOptions<IngredientDTO>(request);

        var dishesResponse = await mediator.Send(new GetAll.Request(), cancellationToken);

        return TypedResults.Ok(await dishesResponse.Dishes.GetPagedResponseAsync(
            mapper,
            request: request,
            queryOptions: queryOptions,
            cancellationToken: cancellationToken)
        );
    }

    public static async Task<Results<NotFound, Ok<GetById.Response>>> GetIngredientByIdAsync(
    [FromServices] IMediator mediator,
    [FromRoute] Guid ingredientId,
    CancellationToken cancellationToken
    )
    {
        var ingredient = await mediator.Send(new GetById.Request(ingredientId), cancellationToken);
        if (ingredient == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(ingredient);
    }

    public static async Task<CreatedAtRoute<IngredientDTO>> CreateIngredientAsync(
    [FromServices] IMediator mediator,
    [FromBody] Create.Request request,
    CancellationToken cancellationToken
)
    {
        var ingredient = await mediator.Send(request, cancellationToken);

        return TypedResults.CreatedAtRoute(
            ingredient,
            "GetIngredient",
            new
            {
                ingredientId = ingredient.Id
            }
        );
    }
}
