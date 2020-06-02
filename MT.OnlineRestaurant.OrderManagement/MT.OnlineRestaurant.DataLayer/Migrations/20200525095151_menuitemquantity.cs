using Microsoft.EntityFrameworkCore.Migrations;

namespace MT.OnlineRestaurant.DataLayer.Migrations
{
    public partial class menuitemquantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "tblFoodOrderMapping",
                nullable: false,
                defaultValueSql: "((0))");

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "tblFoodOrder",
                nullable: false,
                defaultValueSql: "((0))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "quantity",
                table: "tblFoodOrderMapping");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "tblFoodOrder");
        }
    }
}
