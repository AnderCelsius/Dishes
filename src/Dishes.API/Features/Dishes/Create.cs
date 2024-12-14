using AutoMapper;
using Dishes.API.Dispatchers;
using Dishes.API.Exceptions;
using Dishes.Common.Models;
using Dishes.Core.Contracts;
using Dishes.Core.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dishes.API.Features.Dishes;

public static class Create
{
    public record Request(string Name) : IRequest<DishDTO>;

    public class Handler(
        IGenericRepository<Dish> dishRepository,
        IMapper mapper,
        IWebhookEventDispatcher webhookDispatcher
    ) : IRequestHandler<Request, DishDTO>
    {
        private readonly IGenericRepository<Dish> _dishRepository = dishRepository ?? throw new ArgumentException(nameof(dishRepository));

        private readonly IMapper _mapper = mapper ?? throw new ArgumentException(nameof(mapper));

        public async Task<DishDTO> Handle(Request request, CancellationToken cancellationToken)
        {
            var dish = new Dish(request.Name);
            await _dishRepository.InsertAsync(dish);

            await webhookDispatcher.DispatchEventAsync("DishAdded", dish);

            return _mapper.Map<DishDTO>(dish);
        }
    }

    public class CreateDishValidator : AbstractValidator<Request>
    {
        private readonly IGenericRepository<Dish> _dishRepository;

        public CreateDishValidator(IGenericRepository<Dish> dishRepository)
        {
            _dishRepository = dishRepository;

            RuleFor(request => request.Name)
              .NotEmpty().WithMessage("Name is required.")
              .Length(3, 255).WithMessage("Name must be between 3 and 255 characters long.");

            RuleFor(request => request)
                .MustAsync(NameMustBeUnique)
                .WithMessage(_ => "Name of dish must be unique");

        }

        private async Task<bool> NameMustBeUnique(
            Request request,
            CancellationToken cancellationToken
        )
        {
            var potentialDuplicate = await _dishRepository.Table.SingleOrDefaultAsync(d => d.Name == request.Name, cancellationToken);
            if (potentialDuplicate != null)
            {
                throw new DishesDuplicateException($"A different dish with name: '{request.Name}' already exist in record");
            }

            return true;
        }
    }
}
