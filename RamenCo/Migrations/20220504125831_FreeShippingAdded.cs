using Microsoft.EntityFrameworkCore.Migrations;

namespace RamenCo.Migrations
{
    public partial class FreeShippingAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFreeShipping",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFreeShipping",
                table: "Products");
        }
    }
}
