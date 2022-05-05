using Microsoft.EntityFrameworkCore.Migrations;

namespace RamenCo.Migrations
{
    public partial class ProductShipPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ProductShipPrice",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductShipPrice",
                table: "Products");
        }
    }
}
