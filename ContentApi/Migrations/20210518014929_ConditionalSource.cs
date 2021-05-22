using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentApi.Migrations
{
    public partial class ConditionalSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "conditional_sources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    ContentKey = table.Column<Guid>(type: "uuid", nullable: false),
                    JavaScript = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conditional_sources", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "conditional_sources");
        }
    }
}
