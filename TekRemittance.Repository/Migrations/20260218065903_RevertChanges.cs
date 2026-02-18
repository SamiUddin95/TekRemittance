using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RevertChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcquisitionModes",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "DisbursementModes",
                table: "Branches");

            migrationBuilder.AddColumn<string>(
                name: "AcquisitionModes",
                table: "AcquisitionAgents",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DisbursementModes",
                table: "AcquisitionAgents",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcquisitionModes",
                table: "AcquisitionAgents");

            migrationBuilder.DropColumn(
                name: "DisbursementModes",
                table: "AcquisitionAgents");

            migrationBuilder.AddColumn<string>(
                name: "AcquisitionModes",
                table: "Branches",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DisbursementModes",
                table: "Branches",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
