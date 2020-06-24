using Microsoft.EntityFrameworkCore.Migrations;

namespace bkfc.Migrations
{
    public partial class PaymentFoodCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FoodList",
                table: "Payment",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Order",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PaymentFood",
                columns: table => new
                {
                    PaymentId = table.Column<int>(nullable: false),
                    FoodId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentFood", x => new { x.PaymentId, x.FoodId });
                    table.ForeignKey(
                        name: "FK_PaymentFood_Food_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Food",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentFood_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentFood_FoodId",
                table: "PaymentFood",
                column: "FoodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentFood");

            migrationBuilder.DropColumn(
                name: "FoodList",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Order");
        }
    }
}
