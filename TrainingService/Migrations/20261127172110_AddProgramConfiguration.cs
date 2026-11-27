using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingService.Migrations
{
    /// <inheritdoc />
    public partial class AddProgramConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProgramWorkout_TrainingProgram_TrainingProgramId",
                table: "ProgramWorkout");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgramWorkout_Workout_WorkoutId",
                table: "ProgramWorkout");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingProgram",
                table: "TrainingProgram");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProgramWorkout",
                table: "ProgramWorkout");

            migrationBuilder.DropIndex(
                name: "IX_ProgramWorkout_TrainingProgramId",
                table: "ProgramWorkout");

            migrationBuilder.RenameTable(
                name: "TrainingProgram",
                newName: "TrainingPrograms");

            migrationBuilder.RenameTable(
                name: "ProgramWorkout",
                newName: "ProgramWorkouts");

            migrationBuilder.RenameIndex(
                name: "IX_ProgramWorkout_WorkoutId",
                table: "ProgramWorkouts",
                newName: "IX_ProgramWorkouts_WorkoutId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TrainingPrograms",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TrainingPrograms",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Goal",
                table: "TrainingPrograms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingPrograms",
                table: "TrainingPrograms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProgramWorkouts",
                table: "ProgramWorkouts",
                column: "Id");

            migrationBuilder.AddCheckConstraint(
                name: "CK_TrainingProgram_DaysPerWeek",
                table: "TrainingPrograms",
                sql: "DaysPerWeek BETWEEN 1 AND 7");

            migrationBuilder.AddCheckConstraint(
                name: "CK_TrainingProgram_DurationWeeks",
                table: "TrainingPrograms",
                sql: "DurationWeeks >= 1");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramWorkouts_TrainingProgramId_WeekNumber_DayNumber",
                table: "ProgramWorkouts",
                columns: new[] { "TrainingProgramId", "WeekNumber", "DayNumber" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_ProgramWorkout_DayNumber",
                table: "ProgramWorkouts",
                sql: "DayNumber >= 1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ProgramWorkout_WeekNumber",
                table: "ProgramWorkouts",
                sql: "WeekNumber >= 1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramWorkouts_TrainingPrograms_TrainingProgramId",
                table: "ProgramWorkouts",
                column: "TrainingProgramId",
                principalTable: "TrainingPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramWorkouts_Workout_WorkoutId",
                table: "ProgramWorkouts",
                column: "WorkoutId",
                principalTable: "Workout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProgramWorkouts_TrainingPrograms_TrainingProgramId",
                table: "ProgramWorkouts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgramWorkouts_Workout_WorkoutId",
                table: "ProgramWorkouts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingPrograms",
                table: "TrainingPrograms");

            migrationBuilder.DropCheckConstraint(
                name: "CK_TrainingProgram_DaysPerWeek",
                table: "TrainingPrograms");

            migrationBuilder.DropCheckConstraint(
                name: "CK_TrainingProgram_DurationWeeks",
                table: "TrainingPrograms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProgramWorkouts",
                table: "ProgramWorkouts");

            migrationBuilder.DropIndex(
                name: "IX_ProgramWorkouts_TrainingProgramId_WeekNumber_DayNumber",
                table: "ProgramWorkouts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ProgramWorkout_DayNumber",
                table: "ProgramWorkouts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ProgramWorkout_WeekNumber",
                table: "ProgramWorkouts");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "TrainingPrograms");

            migrationBuilder.RenameTable(
                name: "TrainingPrograms",
                newName: "TrainingProgram");

            migrationBuilder.RenameTable(
                name: "ProgramWorkouts",
                newName: "ProgramWorkout");

            migrationBuilder.RenameIndex(
                name: "IX_ProgramWorkouts_WorkoutId",
                table: "ProgramWorkout",
                newName: "IX_ProgramWorkout_WorkoutId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TrainingProgram",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TrainingProgram",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingProgram",
                table: "TrainingProgram",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProgramWorkout",
                table: "ProgramWorkout",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramWorkout_TrainingProgramId",
                table: "ProgramWorkout",
                column: "TrainingProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramWorkout_TrainingProgram_TrainingProgramId",
                table: "ProgramWorkout",
                column: "TrainingProgramId",
                principalTable: "TrainingProgram",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramWorkout_Workout_WorkoutId",
                table: "ProgramWorkout",
                column: "WorkoutId",
                principalTable: "Workout",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
