using AutoMapper;
using Dishes.Common.Models;
using Dishes.Core.Entities;

namespace Dishes.Common.MapProfiles;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<Ingredient, IngredientDTO>().ReverseMap();
    }
}
