using Microsoft.EntityFrameworkCore.Migrations;

namespace bkfc.Migrations
{
    public partial class PaymentAndOrderColumnChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Payment",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Order",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Payment",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Order",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
