using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessProfileService.Migrations
{
    /// <inheritdoc />
    public partial class intialcreate0041 : Migration
    {
        /// <inheritdoc />

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Drop the existing FK before dropping the old PK.
            migrationBuilder.DropForeignKey(
                name: "FK_BodyMeasurements_FitnessProfiles_UserId",
                table: "BodyMeasurements");

            // 2. Drop the old PK.
            migrationBuilder.DropPrimaryKey(
                name: "PK_FitnessProfiles",
                table: "FitnessProfiles");

            // 3. Rename BodyMeasurement PK column.
            migrationBuilder.RenameColumn(
                name: "MeasurementId",
                table: "BodyMeasurements",
                newName: "Id");

            // 4. Alter optional properties.
            migrationBuilder.AlterColumn<string>(
                name: "TrainingExperienceLevel",
                table: "FitnessProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AvailableDaysPerWeek",
                table: "FitnessProfiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            // 5. Add Id as nullable temporarily.
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "FitnessProfiles",
                type: "uniqueidentifier",
                nullable: true);

            // 6. Generate a unique Guid for every existing record.
            migrationBuilder.Sql(
                "UPDATE FitnessProfiles SET Id = NEWID() WHERE Id IS NULL");

            // 7. Make Id required.
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "FitnessProfiles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            // 8. Create the alternate key on UserId.
            migrationBuilder.AddUniqueConstraint(
                name: "AK_FitnessProfiles_UserId",
                table: "FitnessProfiles",
                column: "UserId");

            // 9. Create the new PK.
            migrationBuilder.AddPrimaryKey(
                name: "PK_FitnessProfiles",
                table: "FitnessProfiles",
                column: "Id");

            // 10. Recreate FK referencing the alternate key.
            migrationBuilder.AddForeignKey(
                name: "FK_BodyMeasurements_FitnessProfiles_UserId",
                table: "BodyMeasurements",
                column: "UserId",
                principalTable: "FitnessProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_FitnessProfiles_UserId",
                table: "FitnessProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FitnessProfiles",
                table: "FitnessProfiles");

            migrationBuilder.DropIndex(
                name: "IX_FitnessProfiles_UserId",
                table: "FitnessProfiles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FitnessProfiles");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BodyMeasurements",
                newName: "MeasurementId");

            migrationBuilder.AlterColumn<string>(
                name: "TrainingExperienceLevel",
                table: "FitnessProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AvailableDaysPerWeek",
                table: "FitnessProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FitnessProfiles",
                table: "FitnessProfiles",
                column: "UserId");
        }
    }
}
