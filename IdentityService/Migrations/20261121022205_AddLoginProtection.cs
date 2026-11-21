using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Migrations
{
    /// <inheritdoc />
    public partial class AddLoginProtection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_Token",
                table: "RefreshToken");

            migrationBuilder.AddColumn<string>(
                name: "TokenHash",
                table: "RefreshToken",
                type: "varchar(64)",
                unicode: false,
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            // The old schema did not specify a token format. Retire existing sessions and keep their history.
            migrationBuilder.Sql("""
                UPDATE [RefreshToken]
                SET [TokenHash] = CONVERT(varchar(64), HASHBYTES('SHA2_256', [Token]), 2),
                    [RevokedAt] = COALESCE([RevokedAt], SYSDATETIMEOFFSET());
                """);
            migrationBuilder.DropColumn(name: "Token", table: "RefreshToken");

            migrationBuilder.AddColumn<int>(
                name: "FailedLoginAttempts",
                table: "ApplicationUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "ApplicationUser",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "ApplicationUser",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ApplicationUser",
                type: "rowversion",
                rowVersion: true,
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_TokenHash",
                table: "RefreshToken",
                column: "TokenHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_TokenHash",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "TokenHash",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "FailedLoginAttempts",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ApplicationUser");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "RefreshToken",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            // Plaintext cannot be recovered. Give each historical row a unique, revoked placeholder.
            migrationBuilder.Sql("""
                UPDATE [RefreshToken]
                SET [Token] = CONCAT('revoked-', [TokenId]),
                    [RevokedAt] = COALESCE([RevokedAt], SYSDATETIMEOFFSET());
                """);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Token",
                table: "RefreshToken",
                column: "Token",
                unique: true);
        }
    }
}
