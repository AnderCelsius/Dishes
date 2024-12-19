using AutoMapper;
using Dishes.API.Configuration.OData;
using Dishes.API.Extensions;
using Dishes.API.Features.Dishes;
using Dishes.Common.Models;
using Dishes.Core.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Dishes.API.Endpoints;

public static class DishesEndpoints
{
    [EnableQuery]
    public static async Task<Ok<PagedResponse<DishListDTO>>> GetDishesAsync(
        [FromServices] IMediator mediator,
        [FromServices] IMapper mapper,
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        var queryOptions = ODataQueryOptionsFactory.CreateQueryOptions<DishListDTO>(request);

        var dishesResponse = await mediator.Send(new GetAll.Request(), cancellationToken);

        return TypedResults.Ok(await dishesResponse.Dishes.GetPagedResponseAsync(
            mapper,
            request: request,
            queryOptions: queryOptions,
            cancellationToken: cancellationToken)
        );
    }

    public static async Task<Results<NotFound, Ok<GetById.Response>>> GetDishByIdAsync(
        [FromServices] IMediator mediator,
        [FromRoute] Guid dishId,
        CancellationToken cancellationToken
    )
    {
        var dish = await mediator.Send(new GetById.Request(dishId), cancellationToken);
        if (dish == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(dish);
    }

    [EnableQuery]
    public static async Task<Ok<GetDishIngredients.Response>> GetDishIngredientsAsync(
    [FromServices] IMediator mediator,
    [FromServices] IMapper mapper,
    [FromRoute] Guid dishId,
    HttpRequest request,
    CancellationToken cancellationToken
    ) => TypedResults.Ok(await mediator.Send(new GetDishIngredients.Request(dishId), cancellationToken));

    public static async Task<CreatedAtRoute<DishDTO>> CreateDishAsync(
        [FromServices] IWebhookService webhookService,
        [FromServices] IMediator mediator,
        [FromBody] Create.Request request,
        CancellationToken cancellationToken
    )
    {
        var dish = await mediator.Send(request, cancellationToken);

        return TypedResults.CreatedAtRoute(
            dish,
            "GetDish",
            new
            {
                dishId = dish.Id
            }
        );
    }

    //public static async Task<Results<NotFound, NoContent>> UpdateDishAsync(
    //    DishesDbContext dishesDbContext,
    //    IMapper mapper,
    //    Guid dishId,
    //    DishForUpdateDto dishForUpdateDto
    //)
    //{
    //    var dishEntity = await dishesDbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
    //    if (dishEntity == null)
    //    {
    //        return TypedResults.NotFound();
    //    }

    //    mapper.Map(dishForUpdateDto, dishEntity);
    //    await dishesDbContext.SaveChangesAsync();
    //    return TypedResults.NoContent();
    //}

    //public static async Task<Results<NotFound, NoContent>> UpdateDishWithJsonPatchAsync(
    //    DishesDbContext dishesDbContext,
    //    IMapper mapper,
    //    Guid dishId,
    //    JsonPatchDocument<DishForUpdateDto> patchDoc
    //)
    //{
    //    var dishEntity = await dishesDbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
    //    if (dishEntity == null)
    //    {
    //        return TypedResults.NotFound();
    //    }

    //    var entityDto = mapper.Map<DishForUpdateDto>(dishEntity);
    //    patchDoc.ApplyTo(entityDto);


    //    mapper.Map(entityDto, dishEntity);
    //    await dishesDbContext.SaveChangesAsync();
    //    return TypedResults.NoContent();
    //}

    public static async Task<Results<NotFound, NoContent>> DeleteDishAsync(
        [FromServices] IMediator mediator,
        [FromRoute] Guid dishId
    )
    {
        await mediator.Send(new DeleteById.Request(dishId));
        return TypedResults.NoContent();
    }
}
