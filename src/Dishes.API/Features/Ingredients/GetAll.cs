using Dishes.Core.Contracts;
using Dishes.Core.Entities;
using MediatR;

namespace Dishes.API.Features.Ingredients;

public static class GetAll
{
    public record Request() : IRequest<Response>;

    public record Response(IQueryable<Ingredient> Dishes);

    public class Handler(IGenericRepository<Ingredient> ingredientRepository) : IRequestHandler<Request, Response>
    {
        public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            => Task.FromResult(new Response(ingredientRepository.TableNoTracking));
    }
}