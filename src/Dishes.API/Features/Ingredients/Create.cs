using AutoMapper;
using Dishes.API.Exceptions;
using Dishes.Common.Models;
using Dishes.Core.Contracts;
using Dishes.Core.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dishes.API.Features.Ingredients;

public static class Create
{
    public record Request(string Name) : IRequest<IngredientDTO>;

    public class Handler(
        IGenericRepository<Ingredient> ingredientRepository,
        IMapper mapper
    ) : IRequestHandler<Request, IngredientDTO>
    {
        public async Task<IngredientDTO> Handle(Request request, CancellationToken cancellationToken)
        {
            var ingredient = new Ingredient(request.Name);
            await ingredientRepository.InsertAsync(ingredient);

            return mapper.Map<IngredientDTO>(ingredient);
        }
    }

    public class CreateIngredientValidator : AbstractValidator<Request>
    {
        private readonly IGenericRepository<Ingredient> _ingredientRepository;

        public CreateIngredientValidator(IGenericRepository<Ingredient> ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;

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
            var potentialDuplicate = await _ingredientRepository.Table.SingleOrDefaultAsync(d => d.Name.ToLower() == request.Name.ToLower(), cancellationToken);
            if (potentialDuplicate != null)
            {
                throw new DishesDuplicateException($"A different ingredient with name: '{request.Name}' already exist in record");
            }

            return true;
        }
    }
}
