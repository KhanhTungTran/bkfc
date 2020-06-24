using Microsoft.EntityFrameworkCore.Migrations;

namespace bkfc.Migrations
{
    public partial class PaymentUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodList",
                table: "Payment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FoodList",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
