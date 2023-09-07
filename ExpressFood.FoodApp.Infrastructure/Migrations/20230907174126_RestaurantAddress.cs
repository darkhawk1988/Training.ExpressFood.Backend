﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressFood.FoodApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RestaurantAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Restaurants");
        }
    }
}
