using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationUser_Email",
                table: "ApplicationUser");

            // Legacy codes have no known protection scheme; never carry their values forward.
            migrationBuilder.Sql("""
                IF EXISTS (SELECT 1 FROM [ApplicationUser]
                    WHERE [Email] COLLATE Latin1_General_100_BIN2 LIKE N'%[^ -~]%'
                        OR LEN(LTRIM(RTRIM([Email]))) = 0)
                    THROW 50001, 'Legacy emails require explicit normalization before this migration.', 1;

                IF EXISTS (SELECT 1 FROM [ApplicationUser]
                    GROUP BY UPPER(LTRIM(RTRIM([Email])) COLLATE Latin1_General_100_CI_AS)
                        COLLATE Latin1_General_100_BIN2
                    HAVING COUNT(*) > 1)
                    THROW 50002, 'Email normalization conflicts require manual resolution.', 1;

                UPDATE [OtpCode] SET [Code] = REPLICATE('0', 64), [IsUsed] = 1;
                """);

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "OtpCode",
                newName: "CodeHash");

            migrationBuilder.AddColumn<Guid>(
                name: "ChallengeId",
                table: "OtpCode",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "CodeHash",
                table: "OtpCode",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "OtpCode",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "FailedAttempts",
                table: "OtpCode",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "OtpCode",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ApplicationUser",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "ApplicationUser",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                collation: "Latin1_General_100_BIN2");

            migrationBuilder.Sql("""
                UPDATE [ApplicationUser]
                SET [NormalizedEmail] = UPPER(LTRIM(RTRIM([Email])) COLLATE Latin1_General_100_CI_AS);

                UPDATE [OtpCode]
                SET [ChallengeId] = NEWID(), [CreatedAt] = SYSUTCDATETIME(),
                    [ExpiresAt] = SYSUTCDATETIME(), [Purpose] = 'EmailVerification';
                """);

            migrationBuilder.CreateIndex(
                name: "IX_OtpCode_ChallengeId",
                table: "OtpCode",
                column: "ChallengeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_ApplicationUser_NormalizedEmail",
                table: "ApplicationUser",
                column: "NormalizedEmail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF EXISTS (SELECT 1 FROM [ApplicationUser] WHERE [Name] IS NOT NULL)
                    THROW 50003, 'Rollback would discard account names. Use a forward fix or restore a backup.', 1;
                UPDATE [OtpCode] SET [IsUsed] = 1;
                """);

            migrationBuilder.DropIndex(
                name: "IX_OtpCode_ChallengeId",
                table: "OtpCode");

            migrationBuilder.DropIndex(
                name: "UX_ApplicationUser_NormalizedEmail",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "ChallengeId",
                table: "OtpCode");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "OtpCode");

            migrationBuilder.DropColumn(
                name: "FailedAttempts",
                table: "OtpCode");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "OtpCode");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "ApplicationUser");

            migrationBuilder.AlterColumn<string>(
                name: "CodeHash",
                table: "OtpCode",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.RenameColumn(name: "CodeHash", table: "OtpCode", newName: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Email",
                table: "ApplicationUser",
                column: "Email",
                unique: true);
        }
    }
}
