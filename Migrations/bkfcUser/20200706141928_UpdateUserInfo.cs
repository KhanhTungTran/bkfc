using Microsoft.EntityFrameworkCore.Migrations;

namespace bkfc.Migrations.bkfcUser
{
    public partial class UpdateUserInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "vendorid",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: -1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "vendorid",
                table: "AspNetUsers");
        }
    }
}
