using Dishes.Core.Contracts;
using Dishes.Core.Entities;
using MediatR;

namespace Dishes.API.Features.Dishes;

public static class GetAll
{
    public record Request() : IRequest<Response>;

    public record Response(IQueryable<Dish> Dishes);

    public class Handler(IGenericRepository<Dish> dishRepository) : IRequestHandler<Request, Response>
    {
        private readonly IGenericRepository<Dish> _dishRepository = dishRepository;

        public Task<Response> Handle(Request request, CancellationToken cancellationToken) => Task.FromResult(new Response(_dishRepository.TableNoTracking));
    }
}