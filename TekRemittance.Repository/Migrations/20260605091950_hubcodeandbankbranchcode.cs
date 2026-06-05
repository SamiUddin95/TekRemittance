using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class hubcodeandbankbranchcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HubIds",
                table: "Users",
                newName: "HubCodes");

            migrationBuilder.RenameColumn(
                name: "BankBranchIds",
                table: "Users",
                newName: "BankBranchCodes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HubCodes",
                table: "Users",
                newName: "HubIds");

            migrationBuilder.RenameColumn(
                name: "BankBranchCodes",
                table: "Users",
                newName: "BankBranchIds");
        }
    }
}
