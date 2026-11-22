using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgressService.Migrations
{
    /// <inheritdoc />
    public partial class UniqueCompletedWorkoutSessionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CompletedWorkoutRecords_SessionId",
                table: "CompletedWorkoutRecords",
                column: "SessionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompletedWorkoutRecords_SessionId",
                table: "CompletedWorkoutRecords");
        }
    }
}
