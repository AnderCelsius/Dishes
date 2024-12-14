using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dishes.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class InitializeDb : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Dishes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Dishes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Ingredients",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Ingredients", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "DishIngredients",
            columns: table => new
            {
                DishId = table.Column<Guid>(type: "TEXT", nullable: false),
                IngredientId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DishIngredients", x => new { x.DishId, x.IngredientId });
                table.ForeignKey(
                    name: "FK_DishIngredients_Dishes_DishId",
                    column: x => x.DishId,
                    principalTable: "Dishes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_DishIngredients_Ingredients_IngredientId",
                    column: x => x.IngredientId,
                    principalTable: "Ingredients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_DishIngredients_IngredientId",
            table: "DishIngredients",
            column: "IngredientId");

        migrationBuilder.CreateIndex(
            name: "IX_Dishes_Name",
            table: "Dishes",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_Ingredients_Name",
            table: "Ingredients",
            column: "Name");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "DishIngredients");

        migrationBuilder.DropTable(
            name: "Dishes");

        migrationBuilder.DropTable(
            name: "Ingredients");
    }
}
