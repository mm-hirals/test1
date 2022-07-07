using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MidCapERP.DataEntities.Migrations
{
    public partial class UserModelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultTheme",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "AspNetRoles");

            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "AspNetUsers",
                newName: "IsActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "AspNetUsers",
                newName: "IsEnabled");

            migrationBuilder.AddColumn<string>(
                name: "DefaultTheme",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}