using Microsoft.EntityFrameworkCore.Migrations;

namespace bkfc.Migrations
{
    public partial class ConventionRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderFood_Food_FoodId",
                table: "OrderFood");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderFood_Order_OrderId",
                table: "OrderFood");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentFood_Food_FoodId",
                table: "PaymentFood");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentFood_Payment_PaymentId",
                table: "PaymentFood");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentFood",
                table: "PaymentFood");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderFood",
                table: "OrderFood");

            migrationBuilder.RenameTable(
                name: "PaymentFood",
                newName: "PaymentFoods");

            migrationBuilder.RenameTable(
                name: "OrderFood",
                newName: "OrderFoods");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentFood_FoodId",
                table: "PaymentFoods",
                newName: "IX_PaymentFoods_FoodId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderFood_FoodId",
                table: "OrderFoods",
                newName: "IX_OrderFoods_FoodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentFoods",
                table: "PaymentFoods",
                columns: new[] { "PaymentId", "FoodId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderFoods",
                table: "OrderFoods",
                columns: new[] { "OrderId", "FoodId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderFoods_Food_FoodId",
                table: "OrderFoods",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderFoods_Order_OrderId",
                table: "OrderFoods",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentFoods_Food_FoodId",
                table: "PaymentFoods",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentFoods_Payment_PaymentId",
                table: "PaymentFoods",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderFoods_Food_FoodId",
                table: "OrderFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderFoods_Order_OrderId",
                table: "OrderFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentFoods_Food_FoodId",
                table: "PaymentFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentFoods_Payment_PaymentId",
                table: "PaymentFoods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentFoods",
                table: "PaymentFoods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderFoods",
                table: "OrderFoods");

            migrationBuilder.RenameTable(
                name: "PaymentFoods",
                newName: "PaymentFood");

            migrationBuilder.RenameTable(
                name: "OrderFoods",
                newName: "OrderFood");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentFoods_FoodId",
                table: "PaymentFood",
                newName: "IX_PaymentFood_FoodId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderFoods_FoodId",
                table: "OrderFood",
                newName: "IX_OrderFood_FoodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentFood",
                table: "PaymentFood",
                columns: new[] { "PaymentId", "FoodId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderFood",
                table: "OrderFood",
                columns: new[] { "OrderId", "FoodId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderFood_Food_FoodId",
                table: "OrderFood",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderFood_Order_OrderId",
                table: "OrderFood",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentFood_Food_FoodId",
                table: "PaymentFood",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentFood_Payment_PaymentId",
                table: "PaymentFood",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
