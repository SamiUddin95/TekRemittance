using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class deletecolumnsInHubBankBranhes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CrAccDollar",
                table: "Hub");

            migrationBuilder.DropColumn(
                name: "CrAccIntercity",
                table: "Hub");

            migrationBuilder.DropColumn(
                name: "CrAccNormal",
                table: "Hub");

            migrationBuilder.DropColumn(
                name: "CrAccSameDay",
                table: "Hub");

            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "Hub");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Hub");

            migrationBuilder.DropColumn(
                name: "Email1",
                table: "BankBranches");

            migrationBuilder.DropColumn(
                name: "Email2",
                table: "BankBranches");

            migrationBuilder.DropColumn(
                name: "Email3",
                table: "BankBranches");

            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "BankBranches");

            migrationBuilder.DropColumn(
                name: "NIFTBranchCode",
                table: "BankBranches");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "BankBranches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CrAccDollar",
                table: "Hub",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CrAccIntercity",
                table: "Hub",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CrAccNormal",
                table: "Hub",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CrAccSameDay",
                table: "Hub",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "Hub",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Hub",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email1",
                table: "BankBranches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email2",
                table: "BankBranches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email3",
                table: "BankBranches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "BankBranches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NIFTBranchCode",
                table: "BankBranches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "BankBranches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
