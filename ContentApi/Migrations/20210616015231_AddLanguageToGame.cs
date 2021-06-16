﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentApi.Migrations
{
    public partial class AddLanguageToGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "games",
                type: "text",
                nullable: true,
                defaultValue: "en");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "games");
        }
    }
}
