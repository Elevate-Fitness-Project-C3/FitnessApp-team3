using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RotatedAt",
                table: "RefreshToken",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SessionRootTokenId",
                table: "RefreshToken",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SessionVersion",
                table: "RefreshToken",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_SessionRootTokenId",
                table: "RefreshToken",
                column: "SessionRootTokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_RefreshToken_SessionRootTokenId",
                table: "RefreshToken",
                column: "SessionRootTokenId",
                principalTable: "RefreshToken",
                principalColumn: "TokenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_RefreshToken_SessionRootTokenId",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_SessionRootTokenId",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "RotatedAt",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "SessionRootTokenId",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "SessionVersion",
                table: "RefreshToken");
        }
    }
}
