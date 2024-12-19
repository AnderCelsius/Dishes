using AutoMapper;
using Dishes.API.Exceptions;
using Dishes.Common.Models;
using Dishes.Core.Contracts;
using Dishes.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dishes.API.Features.Ingredients;

public static class GetById
{
    public record Request(Guid IngredientId) : IRequest<Response>;
    public record Response(IngredientDTO Dish);

    public class Handler(
        IGenericRepository<Ingredient> ingredientRepository,
        IMapper mapper
    ) : IRequestHandler<Request, Response>
    {

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var ingredient = await ingredientRepository
                .TableNoTracking
                .SingleOrDefaultAsync(Dish => Dish.Id == request.IngredientId, cancellationToken);

            if (ingredient is null)
            {
                throw new DishesNotFoundException($"Could not find an Ingredient with ID {request.IngredientId}.");
            }

            var result = mapper.Map<IngredientDTO>(ingredient);


            return mapper.Map<Response>(result);
        }
    }
}
