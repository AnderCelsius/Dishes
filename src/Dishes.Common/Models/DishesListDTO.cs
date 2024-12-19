namespace Dishes.Common.Models;

public class DishListDTO : DishDTO
{
    public List<IngredientDTO> Ingredients { get; set; }
}