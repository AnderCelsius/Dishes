using Dishes.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dishes.Infrastructure.Data.Configuration;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(i => i.Name);

        builder.HasMany(i => i.Dishes)
            .WithMany(d => d.Ingredients)
            .UsingEntity<DishIngredient>();
    }
}