using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessProfileService.Migrations
{
    /// <inheritdoc />
    public partial class intialcreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "FitnessProfiles",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnboardingCompleted",
                table: "FitnessProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnboardingCompleted",
                table: "FitnessProfiles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "FitnessProfiles",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
