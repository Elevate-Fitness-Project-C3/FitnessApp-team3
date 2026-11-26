using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityService.Migrations;

public partial class UseGuidIdentityKeys : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey("FK_OtpCode_ApplicationUser_UserId", "OtpCode");
        migrationBuilder.DropForeignKey("FK_RefreshToken_ApplicationUser_UserId", "RefreshToken");
        migrationBuilder.DropForeignKey("FK_RefreshToken_RefreshToken_SessionRootTokenId", "RefreshToken");

        migrationBuilder.DropIndex("IX_OtpCode_UserId", "OtpCode");
        migrationBuilder.DropIndex("IX_RefreshToken_UserId", "RefreshToken");
        migrationBuilder.DropIndex("IX_RefreshToken_SessionRootTokenId", "RefreshToken");

        migrationBuilder.DropPrimaryKey("PK_ApplicationUser", "ApplicationUser");
        migrationBuilder.DropPrimaryKey("PK_OtpCode", "OtpCode");
        migrationBuilder.DropPrimaryKey("PK_RefreshToken", "RefreshToken");

        migrationBuilder.DropColumn("UserId", "ApplicationUser");
        migrationBuilder.DropColumn("UserId", "OtpCode");
        migrationBuilder.DropColumn("UserId", "RefreshToken");
        migrationBuilder.DropColumn("TokenId", "RefreshToken");
        migrationBuilder.DropColumn("SessionRootTokenId", "RefreshToken");

        // Keep the existing identity sequence for ordering challenges independently of their keys.
        migrationBuilder.RenameColumn("OtpId", "OtpCode", "Sequence");
        migrationBuilder.RenameColumn("GuidId", "ApplicationUser", "UserId");
        migrationBuilder.RenameColumn("GuidId", "OtpCode", "OtpId");
        migrationBuilder.RenameColumn("GuidUserId", "OtpCode", "UserId");
        migrationBuilder.RenameColumn("GuidId", "RefreshToken", "TokenId");
        migrationBuilder.RenameColumn("GuidUserId", "RefreshToken", "UserId");
        migrationBuilder.RenameColumn("GuidSessionRootTokenId", "RefreshToken", "SessionRootTokenId");

        // Changing these columns to NOT NULL rejects any rows that were not migrated.
        migrationBuilder.AlterColumn<Guid>("UserId", "ApplicationUser", "uniqueidentifier", nullable: false,
            oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);
        migrationBuilder.AlterColumn<Guid>("OtpId", "OtpCode", "uniqueidentifier", nullable: false,
            oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);
        migrationBuilder.AlterColumn<Guid>("UserId", "OtpCode", "uniqueidentifier", nullable: false,
            oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);
        migrationBuilder.AlterColumn<Guid>("TokenId", "RefreshToken", "uniqueidentifier", nullable: false,
            oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);
        migrationBuilder.AlterColumn<Guid>("UserId", "RefreshToken", "uniqueidentifier", nullable: false,
            oldClrType: typeof(Guid), oldType: "uniqueidentifier", oldNullable: true);

        migrationBuilder.AddPrimaryKey("PK_ApplicationUser", "ApplicationUser", "UserId");
        migrationBuilder.AddPrimaryKey("PK_OtpCode", "OtpCode", "OtpId");
        migrationBuilder.AddPrimaryKey("PK_RefreshToken", "RefreshToken", "TokenId");
        migrationBuilder.CreateIndex("IX_OtpCode_UserId", "OtpCode", "UserId");
        migrationBuilder.CreateIndex("IX_RefreshToken_UserId", "RefreshToken", "UserId");
        migrationBuilder.CreateIndex("IX_RefreshToken_SessionRootTokenId", "RefreshToken", "SessionRootTokenId");

        migrationBuilder.AddForeignKey("FK_OtpCode_ApplicationUser_UserId", "OtpCode", "UserId",
            "ApplicationUser", principalColumn: "UserId", onDelete: ReferentialAction.Cascade);
        migrationBuilder.AddForeignKey("FK_RefreshToken_ApplicationUser_UserId", "RefreshToken", "UserId",
            "ApplicationUser", principalColumn: "UserId", onDelete: ReferentialAction.Cascade);
        migrationBuilder.AddForeignKey("FK_RefreshToken_RefreshToken_SessionRootTokenId", "RefreshToken",
            "SessionRootTokenId", "RefreshToken", principalColumn: "TokenId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
        => throw new NotSupportedException("Restore the pre-upgrade database backup to recover integer Identity keys.");
}
