using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class oneToManyForWorkoutCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Workout_Catalog",
                table: "Workouts");

            migrationBuilder.CreateIndex(
                name: "IX_Workout_CatalogId",
                table: "Workouts",
                column: "CatalogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Workout_CatalogId",
                table: "Workouts");

            migrationBuilder.CreateIndex(
                name: "IX_Workout_Catalog",
                table: "Workouts",
                column: "CatalogId",
                unique: true);
        }
    }
}
