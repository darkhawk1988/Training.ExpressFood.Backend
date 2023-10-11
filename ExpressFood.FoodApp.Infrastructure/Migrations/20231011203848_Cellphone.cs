using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressFood.FoodApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Cellphone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cellphone",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cellphone",
                table: "ApplicationUsers");
        }
    }
}
