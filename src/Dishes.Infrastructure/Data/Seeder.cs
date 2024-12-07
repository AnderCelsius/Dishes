using Dishes.Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dishes.Infrastructure.Data;

public static class Seeder
{
  public static async Task EnsureSeedAppData(WebApplication app)
  {
    using var scope = app.Services.GetRequiredService<IServiceScopeFactory>()
        .CreateScope();
    var context = scope.ServiceProvider.GetService<DishesDbContext>();
    await context!.Database.MigrateAsync();

    await SeedIngredients(context);
    await SeedDishes(context);
    await SeedDishIngredients(context);
  }

  private static async Task SeedIngredients(DishesDbContext context)
  {
    if (!context.Ingredients.Any())
    {
      context.Ingredients.AddRange(
          new Ingredient(Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), "Beef"),
          new Ingredient(Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), "Onion"),
          new Ingredient(Guid.Parse("c19099ed-94db-44ba-885b-0ad7205d5e40"), "Dark beer"),
          new Ingredient(Guid.Parse("0c4dc798-b38b-4a1c-905c-a9e76dbef17b"), "Brown piece of bread"),
          new Ingredient(Guid.Parse("937b1ba1-7969-4324-9ab5-afb0e4d875e6"), "Mustard"),
          new Ingredient(Guid.Parse("7a2fbc72-bb33-49de-bd23-c78fceb367fc"), "Chicory"),
          new Ingredient(Guid.Parse("b5f336e2-c226-4389-aac3-2499325a3de9"), "Mayo"),
          new Ingredient(Guid.Parse("c22bec27-a880-4f2a-b380-12dcd99c61fe"), "Various spices"),
          new Ingredient(Guid.Parse("aab31c70-57ce-4b6d-a66c-9c1b094e915d"), "Mussels"),
          new Ingredient(Guid.Parse("fef8b722-664d-403f-ae3c-05f8ed7d7a1f"), "Celery"),
          new Ingredient(Guid.Parse("8d5a1b40-6677-4545-b6e8-5ba93efda0a1"), "French fries"),
          new Ingredient(Guid.Parse("40563e5b-e538-4084-9587-3df74fae21d4"), "Tomato"),
          new Ingredient(Guid.Parse("f350e1a0-38de-42fe-ada5-ae436378ee5b"), "Tomato paste"),
          new Ingredient(Guid.Parse("d5cad9a4-144e-4a3d-858d-9840792fa65d"), "Bay leave"),
          new Ingredient(Guid.Parse("b617df23-3d91-40e1-99aa-b07d264aa937"), "Carrot"),
          new Ingredient(Guid.Parse("b8b9a6ae-9bcc-4fb3-b883-5974e04eda56"), "Garlic"),
          new Ingredient(Guid.Parse("ecd396c3-4403-4fbf-83ca-94a8e9d859b3"), "Red wine"),
          new Ingredient(Guid.Parse("c2c75b40-2453-416e-a7ed-3505b121d671"), "Coconut milk"),
          new Ingredient(Guid.Parse("3bd3f0a1-87d3-4b85-94fa-ba92bd1874e7"), "Ginger"),
          new Ingredient(Guid.Parse("047ab5cc-d041-486e-9d22-a0860fb13237"), "Chili pepper"),
          new Ingredient(Guid.Parse("e0017fe1-773f-4a59-9730-9489833c6e8e"), "Tamarind paste"),
          new Ingredient(Guid.Parse("c9b46f9c-d6ce-42c3-8736-2cddbbadee10"), "Firm fish"),
          new Ingredient(Guid.Parse("a07cde83-3127-45da-bbd5-04a7c8d13aa4"), "Ginger garlic paste"),
          new Ingredient(Guid.Parse("ebe94d5d-2ad8-4886-b246-05a1fad83d1c"), "Garam masala"));

      await context.SaveChangesAsync();
    }
  }

  private static async Task SeedDishes(DishesDbContext context)
  {
    if (!context.Dishes.Any())
    {
      context.Dishes.AddRange(
          new Dish(Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"),
              "Flemish Beef stew with chicory"),
          new Dish(Guid.Parse("fe462ec7-b30c-4987-8a8e-5f7dbd8e0cfa"),
              "Mussels with french fries"),
          new Dish(Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"),
              "Ragu alla bolognaise"),
          new Dish(Guid.Parse("fd630a57-2352-4731-b25c-db9cc7601b16"),
              "Rendang"),
          new Dish(Guid.Parse("98929bd4-f099-41eb-a994-f1918b724b5a"),
              "Fish Masala"));

      await context.SaveChangesAsync();
    }
  }

  private static async Task SeedDishIngredients(DishesDbContext context)
  {
    if (!context.DishIngredients.Any())
    {
      context.DishIngredients.AddRange(
          new DishIngredient
          {
            DishId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"),
            IngredientId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"),
            IngredientId = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"),
            IngredientId = Guid.Parse("c19099ed-94db-44ba-885b-0ad7205d5e40")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"),
            IngredientId = Guid.Parse("0c4dc798-b38b-4a1c-905c-a9e76dbef17b")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"),
            IngredientId = Guid.Parse("937b1ba1-7969-4324-9ab5-afb0e4d875e6")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"),
            IngredientId = Guid.Parse("7a2fbc72-bb33-49de-bd23-c78fceb367fc")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"),
            IngredientId = Guid.Parse("b5f336e2-c226-4389-aac3-2499325a3de9")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"),
            IngredientId = Guid.Parse("c22bec27-a880-4f2a-b380-12dcd99c61fe")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"),
            IngredientId = Guid.Parse("d5cad9a4-144e-4a3d-858d-9840792fa65d")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("fe462ec7-b30c-4987-8a8e-5f7dbd8e0cfa"),
            IngredientId = Guid.Parse("aab31c70-57ce-4b6d-a66c-9c1b094e915d")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("fe462ec7-b30c-4987-8a8e-5f7dbd8e0cfa"),
            IngredientId = Guid.Parse("fef8b722-664d-403f-ae3c-05f8ed7d7a1f")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("fe462ec7-b30c-4987-8a8e-5f7dbd8e0cfa"),
            IngredientId = Guid.Parse("8d5a1b40-6677-4545-b6e8-5ba93efda0a1")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("fe462ec7-b30c-4987-8a8e-5f7dbd8e0cfa"),
            IngredientId = Guid.Parse("c22bec27-a880-4f2a-b380-12dcd99c61fe")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("fe462ec7-b30c-4987-8a8e-5f7dbd8e0cfa"),
            IngredientId = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"),
            IngredientId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"),
            IngredientId = Guid.Parse("40563e5b-e538-4084-9587-3df74fae21d4")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"),
            IngredientId = Guid.Parse("f350e1a0-38de-42fe-ada5-ae436378ee5b")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"),
            IngredientId = Guid.Parse("d5cad9a4-144e-4a3d-858d-9840792fa65d")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"),
            IngredientId = Guid.Parse("fef8b722-664d-403f-ae3c-05f8ed7d7a1f")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"),
            IngredientId = Guid.Parse("b617df23-3d91-40e1-99aa-b07d264aa937")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"),
            IngredientId = Guid.Parse("b8b9a6ae-9bcc-4fb3-b883-5974e04eda56")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"),
            IngredientId = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"),
            IngredientId = Guid.Parse("ecd396c3-4403-4fbf-83ca-94a8e9d859b3")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"),
            IngredientId = Guid.Parse("c22bec27-a880-4f2a-b380-12dcd99c61fe")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("fd630a57-2352-4731-b25c-db9cc7601b16"),
            IngredientId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("fd630a57-2352-4731-b25c-db9cc7601b16"),
            IngredientId = Guid.Parse("c2c75b40-2453-416e-a7ed-3505b121d671")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("fd630a57-2352-4731-b25c-db9cc7601b16"),
            IngredientId = Guid.Parse("b8b9a6ae-9bcc-4fb3-b883-5974e04eda56")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("fd630a57-2352-4731-b25c-db9cc7601b16"),
            IngredientId = Guid.Parse("3bd3f0a1-87d3-4b85-94fa-ba92bd1874e7")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("fd630a57-2352-4731-b25c-db9cc7601b16"),
            IngredientId = Guid.Parse("047ab5cc-d041-486e-9d22-a0860fb13237")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("fd630a57-2352-4731-b25c-db9cc7601b16"),
            IngredientId = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("fd630a57-2352-4731-b25c-db9cc7601b16"),
            IngredientId = Guid.Parse("e0017fe1-773f-4a59-9730-9489833c6e8e")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("fd630a57-2352-4731-b25c-db9cc7601b16"),
            IngredientId = Guid.Parse("c22bec27-a880-4f2a-b380-12dcd99c61fe")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("98929bd4-f099-41eb-a994-f1918b724b5a"),
            IngredientId = Guid.Parse("c9b46f9c-d6ce-42c3-8736-2cddbbadee10")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("98929bd4-f099-41eb-a994-f1918b724b5a"),
            IngredientId = Guid.Parse("a07cde83-3127-45da-bbd5-04a7c8d13aa4")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("98929bd4-f099-41eb-a994-f1918b724b5a"),
            IngredientId = Guid.Parse("ebe94d5d-2ad8-4886-b246-05a1fad83d1c")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("98929bd4-f099-41eb-a994-f1918b724b5a"),
            IngredientId = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("98929bd4-f099-41eb-a994-f1918b724b5a"),
            IngredientId = Guid.Parse("40563e5b-e538-4084-9587-3df74fae21d4")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("98929bd4-f099-41eb-a994-f1918b724b5a"),
            IngredientId = Guid.Parse("c2c75b40-2453-416e-a7ed-3505b121d671")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("98929bd4-f099-41eb-a994-f1918b724b5a"),
            IngredientId = Guid.Parse("d5cad9a4-144e-4a3d-858d-9840792fa65d")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("98929bd4-f099-41eb-a994-f1918b724b5a"),
            IngredientId = Guid.Parse("047ab5cc-d041-486e-9d22-a0860fb13237")
          },
          new DishIngredient
          {
            DishId = Guid.Parse("98929bd4-f099-41eb-a994-f1918b724b5a"),
            IngredientId = Guid.Parse("c22bec27-a880-4f2a-b380-12dcd99c61fe")
          });

      await context.SaveChangesAsync();
    }
  }
}