using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgressService.Migrations
{
    /// <inheritdoc />
    public partial class HistoryIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CompletedWorkoutRecords_UserId_CompletedAtUtc_Id",
                table: "CompletedWorkoutRecords",
                columns: new[] { "UserId", "CompletedAtUtc", "Id" },
                descending: new[] { false, true, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompletedWorkoutRecords_UserId_CompletedAtUtc_Id",
                table: "CompletedWorkoutRecords");
        }
    }
}
