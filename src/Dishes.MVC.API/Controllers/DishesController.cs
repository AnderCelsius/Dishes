using AutoMapper;
using Dishes.Common.Authorization;
using Dishes.Common.Models;
using Dishes.Core.Entities;
using Dishes.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dishes.MVC.API.Controllers
{

    /// <summary>
    /// Controller for managing dishes in the application.
    /// </summary>
    /// <remarks>
    /// The DishesController provides endpoints for CRUD operations on dishes.
    /// It leverages Entity Framework for data access and AutoMapper for object mapping.
    /// This controller requires user authentication and specific actions require admin privileges.
    /// </remarks>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DishesController(
        DishesDbContext dishesDbContext,
        ILogger<DishesController> logger,
        IMapper mapper) : ControllerBase
    {

        /// <summary>
        /// Retrieves a list of dishes. Optionally filters by name.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all dishes from the database. If a name is provided,
        /// it returns only those dishes that contain the given name in their titles.
        /// </remarks>
        /// <param name="name">The optional name to filter the dishes.</param>
        /// <returns>A list of dish DTOs.</returns>
        [HttpGet(Name = "GetDishesAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DishDto>>> GetDishesAsync(string? name)
        {
            Console.WriteLine($"User authenticated? {HttpContext.User.Identity?.IsAuthenticated}");

            logger.LogInformation("Getting the dishes...");

            return Ok(mapper.Map<IEnumerable<DishDto>>(await dishesDbContext.Dishes
                .Where(d => name == null || d.Name.Contains(name))
                .ToListAsync()));
        }


        /// <summary>
        /// Retrieves a specific dish by its unique identifier.
        /// </summary>
        /// <remarks>
        /// This method searches for a dish using its GUID. If the dish is not found,
        /// a 404 Not Found response is returned. This is used to fetch details of a single dish.
        /// </remarks>
        /// <param name="dishId">The unique identifier of the dish.</param>
        /// <returns>The dish DTO if found; otherwise, NotFound.</returns>
        [HttpGet("{dishId:guid}", Name = "GetDishByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DishDto>> GetDishByIdAsync(Guid dishId)
        {
            var dishEntity = await dishesDbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);

            return dishEntity == null ? NotFound() : Ok(mapper.Map<DishDto>(dishEntity));
        }


        /// <summary>
        /// Retrieves a specific dish by its name.
        /// </summary>
        /// <remarks>
        /// This method allows retrieval of a dish by its name. It is case-sensitive
        /// and returns the first match found. If no match is found, it returns a 404 Not Found.
        /// </remarks>
        /// <param name="dishName">The name of the dish.</param>
        /// <returns>The dish DTO if found; otherwise, NotFound.</returns>
        [HttpGet("{dishName}", Name = "GetDishByNameAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DishDto>> GetDishByNameAsync(string dishName)
        {
            var dishEntity = await dishesDbContext.Dishes.FirstOrDefaultAsync(d => d.Name == dishName);
            return dishEntity == null ? NotFound() : Ok(mapper.Map<DishDto>(dishEntity));
        }


        /// <summary>
        /// Creates a new dish.
        /// </summary>
        /// <remarks>
        /// This method creates a new dish entity in the database. It requires admin privileges.
        /// The body of the request must contain all the necessary creation details.
        /// </remarks>
        /// <param name="dishForCreationDto">The DTO containing dish creation information.</param>
        /// <returns>The created dish DTO.</returns>
        [Authorize(Policy = Policies.RequireAdminFromNigeria)]
        [HttpPost(Name = "CreateDishAsync")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<DishDto>> CreateDishAsync(DishForCreationDto dishForCreationDto)
        {
            var dishEntity = mapper.Map<Dish>(dishForCreationDto);
            dishesDbContext.Add(dishEntity);
            await dishesDbContext.SaveChangesAsync();

            var dishToReturn = mapper.Map<DishDto>(dishEntity);

            return CreatedAtRoute(
                nameof(GetDishByIdAsync),
                new
                {
                    dishId = dishToReturn.Id
                },
                dishToReturn);
        }


        /// <summary>
        /// Updates an existing dish.
        /// </summary>
        /// <remarks>
        /// This endpoint allows updating details of an existing dish using its GUID.
        /// If the dish does not exist, it returns a 404 Not Found. Only the fields specified
        /// in the DTO are updated, and it requires admin privileges.
        /// </remarks>
        /// <param name="dishId">The unique identifier of the dish to be updated.</param>
        /// <param name="dishForUpdateDto">The DTO containing dish update information.</param>
        /// <returns>NoContent if the update is successful; otherwise, NotFound.</returns>
        [HttpPut("{dishId}", Name = "UpdateDishAsync")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateDishAsync(Guid dishId, DishForUpdateDto dishForUpdateDto)
        {
            var dishEntity = await dishesDbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
            if (dishEntity == null)
            {
                return NotFound();
            }

            mapper.Map(dishForUpdateDto, dishEntity);
            await dishesDbContext.SaveChangesAsync();
            return NoContent();
        }


        /// <summary>
        /// Deletes a specific dish by its unique identifier.
        /// </summary>
        /// <remarks>
        /// This method allows deletion of a dish using its GUID. If the dish is not found,
        /// it returns a 404 Not Found. This action requires admin privileges.
        /// </remarks>
        /// <param name="dishId">The unique identifier of the dish to be deleted.</param>
        /// <returns>NoContent if the deletion is successful; otherwise, NotFound.</returns>
        [HttpDelete("{dishId}", Name = "DeleteDishAsync")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteDishAsync(Guid dishId)
        {
            var dishEntity = await dishesDbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
            if (dishEntity == null)
            {
                return NotFound();
            }

            dishesDbContext.Dishes.Remove(dishEntity);
            await dishesDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}