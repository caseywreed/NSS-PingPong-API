using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NSSPingPongBackend.Migrations
{
    public partial class NewPercentageStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DoublesWinPercentage",
                table: "Stats",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SinglesWinPercentage",
                table: "Stats",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoublesWinPercentage",
                table: "Stats");

            migrationBuilder.DropColumn(
                name: "SinglesWinPercentage",
                table: "Stats");
        }
    }
}
