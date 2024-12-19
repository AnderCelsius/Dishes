using Dishes.API.Exceptions;
using Dishes.Core.Contracts;
using Dishes.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dishes.API.Features.Dishes;

public static class DeleteById
{
    public record Request(Guid DishId) : IRequest;

    public class Handler(IGenericRepository<Dish> dishRepository) : IRequestHandler<Request>
    {
        public async Task Handle(Request request, CancellationToken cancellationToken)
        {
            var book = await dishRepository
                .Table
                .SingleOrDefaultAsync(book => book.Id == request.DishId, cancellationToken) ??
                       throw new DishesNotFoundException($"Could not find a dish with ID {request.DishId}.");

            await dishRepository.DeleteAsync(book);
        }
    }
}
