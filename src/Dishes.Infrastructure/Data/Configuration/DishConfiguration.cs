using Dishes.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dishes.Infrastructure.Data.Configuration;

public class DishConfiguration : IEntityTypeConfiguration<Dish>
{
    public void Configure(EntityTypeBuilder<Dish> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(d => d.Name);

        builder
            .HasMany(e => e.Ingredients)
            .WithMany(e => e.Dishes)
            .UsingEntity<DishIngredient>();
    }
}