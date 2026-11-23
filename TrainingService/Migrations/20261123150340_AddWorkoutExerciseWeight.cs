using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingService.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutExerciseWeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "WorkoutExercise",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "WorkoutExercise");
        }
    }
}
