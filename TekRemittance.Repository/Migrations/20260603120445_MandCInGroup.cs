using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class MandCInGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankBranchIds",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HubIds",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "MakerAndChecker",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MakerAndChecker",
                table: "Groups");

            migrationBuilder.AddColumn<string>(
                name: "BankBranchIds",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HubIds",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
