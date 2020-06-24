using Microsoft.EntityFrameworkCore.Migrations;

namespace bkfc.Migrations
{
    public partial class PaymentColumnRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Payment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
