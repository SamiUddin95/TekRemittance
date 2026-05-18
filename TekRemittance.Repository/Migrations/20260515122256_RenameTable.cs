using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RenameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationConfigs",
                table: "ApplicationConfigs");

            migrationBuilder.RenameTable(
                name: "ApplicationConfigs",
                newName: "ApplicationConfig");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationConfig",
                table: "ApplicationConfig",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationConfig",
                table: "ApplicationConfig");

            migrationBuilder.RenameTable(
                name: "ApplicationConfig",
                newName: "ApplicationConfigs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationConfigs",
                table: "ApplicationConfigs",
                column: "Id");
        }
    }
}
