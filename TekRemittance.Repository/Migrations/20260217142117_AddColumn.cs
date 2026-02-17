using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcquisitionModes",
                table: "AcquisitionAgents");

            migrationBuilder.DropColumn(
                name: "DisbursementModes",
                table: "AcquisitionAgents");

            migrationBuilder.AddColumn<bool>(
                name: "IsFT",
                table: "RemittanceInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFT",
                table: "RemittanceInfos");

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
    }
}
