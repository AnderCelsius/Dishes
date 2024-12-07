using Dishes.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Dishes.Infrastructure;

public class DishesDbContext : DbContext
{
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<DishIngredient> DishIngredients => Set<DishIngredient>();


    public DishesDbContext(DbContextOptions<DishesDbContext> options)
          : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
