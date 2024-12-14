using AutoMapper;
using Dishes.API.Exceptions;
using Dishes.Common.Models;
using Dishes.Core.Contracts;
using Dishes.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dishes.API.Features.Dishes;

public static class GetById
{
    public record Request(Guid DishId) : IRequest<Response>;
    public record Response(DishDTO Dish);

    public class Handler(
        IGenericRepository<Dish> DishRepository,
        IMapper mapper
    ) : IRequestHandler<Request, Response>
    {
        private readonly IGenericRepository<Dish> _DishRepository = DishRepository ?? throw new ArgumentException(nameof(DishRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentException(nameof(mapper));

        /// <summary>
        /// Handles the incoming request to retrieve a Dish by its ID.
        /// </summary>
        /// <param name="request">The request to retrieve a Dish by its ID.</param>
        /// <param name="cancellationToken">A token for cancelling the operation if necessary.</param>
        /// <returns>A task representing the asynchronous operation, with a result of the response containing the details of the requested Dish.</returns>
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var dish = await _DishRepository
                .TableNoTracking
                .SingleOrDefaultAsync(Dish => Dish.Id == request.DishId, cancellationToken);

            if (dish is null)
            {
                throw new DishesNotFoundException($"Could not find a Dish with ID {request.DishId}.");
            }

            var result = _mapper.Map<DishDTO>(dish);


            return _mapper.Map<Response>(result);
        }
    }
}
