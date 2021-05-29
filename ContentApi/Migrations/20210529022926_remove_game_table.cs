using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentApi.Migrations
{
    public partial class remove_game_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contents_games_GameId",
                table: "contents");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropIndex(
                name: "IX_contents_GameId",
                table: "contents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_contents_GameId",
                table: "contents",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_contents_games_GameId",
                table: "contents",
                column: "GameId",
                principalTable: "games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
