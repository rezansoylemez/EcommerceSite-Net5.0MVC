using Microsoft.EntityFrameworkCore.Migrations;

namespace RamenCo.Migrations
{
    public partial class ImmediateDeliveryAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsImmediateDelivery",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsImmediateDelivery",
                table: "Products");
        }
    }
}
