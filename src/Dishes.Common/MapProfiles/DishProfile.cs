using AutoMapper;
using Dishes.Common.Models;
using Dishes.Core.Entities;

namespace Dishes.Common.MapProfiles;

public class DishProfile : Profile
{
    public DishProfile()
    {
        CreateMap<Dish, DishDto>();
        CreateMap<DishForCreationDto, Dish>();
        CreateMap<DishForUpdateDto, Dish>();
    }
}
