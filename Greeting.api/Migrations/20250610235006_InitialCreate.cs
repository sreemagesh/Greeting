using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greeting.api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Greetings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Greeting = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Greetings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Greetings",
                columns: new[] { "Id", "CreatedAt", "Greeting", "Name" },
                values: new object[] { 1, new DateTime(2025, 6, 10, 23, 50, 5, 424, DateTimeKind.Utc).AddTicks(4544), "Hello, World", "World" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Greetings");
        }
    }
}
