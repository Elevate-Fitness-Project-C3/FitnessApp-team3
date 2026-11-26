using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityService.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration(Id)]
public class PrepareGuidIdentityKeys : Migration
{
    public const string Id = "20261126000000_PrepareGuidIdentityKeys";

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>("GuidId", "ApplicationUser", "uniqueidentifier", nullable: true);
        migrationBuilder.AddColumn<Guid>("GuidId", "OtpCode", "uniqueidentifier", nullable: true);
        migrationBuilder.AddColumn<Guid>("GuidUserId", "OtpCode", "uniqueidentifier", nullable: true);
        migrationBuilder.AddColumn<Guid>("GuidId", "RefreshToken", "uniqueidentifier", nullable: true);
        migrationBuilder.AddColumn<Guid>("GuidUserId", "RefreshToken", "uniqueidentifier", nullable: true);
        migrationBuilder.AddColumn<Guid>("GuidSessionRootTokenId", "RefreshToken", "uniqueidentifier", nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn("GuidId", "ApplicationUser");
        migrationBuilder.DropColumn("GuidId", "OtpCode");
        migrationBuilder.DropColumn("GuidUserId", "OtpCode");
        migrationBuilder.DropColumn("GuidId", "RefreshToken");
        migrationBuilder.DropColumn("GuidUserId", "RefreshToken");
        migrationBuilder.DropColumn("GuidSessionRootTokenId", "RefreshToken");
    }
}
