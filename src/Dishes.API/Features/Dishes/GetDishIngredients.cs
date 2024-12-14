using AutoMapper;
using Dishes.Common.Models;
using Dishes.Core.Contracts;
using Dishes.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dishes.API.Features.Dishes;

public static class GetDishIngredients
{
    public record Request(Guid DishId) : IRequest<Response>;
    public record Response(IReadOnlyCollection<IngredientDTO> Ingredients);

    public class Handler(IGenericRepository<Dish> repository, IMapper mapper)
        : IRequestHandler<Request, Response>
    {
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var ingredients = await repository
                .TableNoTracking
                .Where(i => i.Id == request.DishId)
                .SelectMany(x => x.Ingredients)
                .ToListAsync();

            var result = mapper.Map<List<IngredientDTO>>(ingredients);

            return new Response(result);
        }
    }
}
