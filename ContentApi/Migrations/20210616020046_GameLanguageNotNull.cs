using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentApi.Migrations
{
    public partial class GameLanguageNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "games",
                type: "text",
                nullable: false,
                defaultValue: "en",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "en");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "games",
                type: "text",
                nullable: true,
                defaultValue: "en",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "en");
        }
    }
}
