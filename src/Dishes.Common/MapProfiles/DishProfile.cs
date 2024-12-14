using AutoMapper;
using Dishes.Common.Models;
using Dishes.Core.Entities;

namespace Dishes.Common.MapProfiles;

public class DishProfile : Profile
{
    public DishProfile()
    {
        CreateMap<Dish, DishDTO>().ReverseMap();
        CreateMap<Dish, DishListDTO>().ReverseMap();
        CreateMap<DishRequestDTO, Dish>();
    }
}
