using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnStatusInRepositoryInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "RemittanceInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "RemittanceInfos");
        }
    }
}
