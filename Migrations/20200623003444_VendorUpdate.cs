using Microsoft.EntityFrameworkCore.Migrations;

namespace bkfc.Migrations
{
    public partial class VendorUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "FoodList",
                table: "Vendor");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Vendor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Vendor");

            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "Vendor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FoodList",
                table: "Vendor",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
