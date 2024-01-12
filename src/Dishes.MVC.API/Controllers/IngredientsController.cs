using AutoMapper;
using Dishes.Common.Models;
using Dishes.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dishes.MVC.API.Controllers;

[ApiController]
[Route("dishes")]
public class IngredientsController(
  DishesDbContext dishesDbContext,
  IMapper mapper) : ControllerBase
{

  [HttpGet("{dishId:guid}/ingredients")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredientsAsync(
    Guid dishId)
  {
    var dishEntity = await dishesDbContext.Dishes
      .Include(d => d.Ingredients)
      .FirstOrDefaultAsync(d => d.Id == dishId);

    if (dishEntity == null)
    {
      return NotFound();
    }

    return Ok(mapper.Map<IEnumerable<IngredientDto>>(dishEntity.Ingredients));
  }
}