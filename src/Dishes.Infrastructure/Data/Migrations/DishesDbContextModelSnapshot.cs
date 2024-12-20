﻿// <auto-generated />
using System;
using Dishes.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dishes.Infrastructure.Data.Migrations;

[DbContext(typeof(DishesDbContext))]
partial class DishesDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

        modelBuilder.Entity("Dishes.Core.Entities.Dish", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("Name");

                b.ToTable("Dishes");
            });

        modelBuilder.Entity("Dishes.Core.Entities.DishIngredient", b =>
            {
                b.Property<Guid>("DishId")
                    .HasColumnType("TEXT");

                b.Property<Guid>("IngredientId")
                    .HasColumnType("TEXT");

                b.HasKey("DishId", "IngredientId");

                b.HasIndex("IngredientId");

                b.ToTable("DishIngredients", (string)null);
            });

        modelBuilder.Entity("Dishes.Core.Entities.Ingredient", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.HasIndex("Name");

                b.ToTable("Ingredients");
            });

        modelBuilder.Entity("Dishes.Core.Entities.DishIngredient", b =>
            {
                b.HasOne("Dishes.Core.Entities.Dish", "Dish")
                    .WithMany("DishIngredients")
                    .HasForeignKey("DishId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Dishes.Core.Entities.Ingredient", "Ingredient")
                    .WithMany("DishIngredients")
                    .HasForeignKey("IngredientId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Dish");

                b.Navigation("Ingredient");
            });

        modelBuilder.Entity("Dishes.Core.Entities.Dish", b =>
            {
                b.Navigation("DishIngredients");
            });

        modelBuilder.Entity("Dishes.Core.Entities.Ingredient", b =>
            {
                b.Navigation("DishIngredients");
            });
#pragma warning restore 612, 618
    }
}
