using Microsoft.EntityFrameworkCore.Migrations;

namespace bkfc.Migrations
{
    public partial class OrderColumnRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodList",
                table: "Order");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FoodList",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
