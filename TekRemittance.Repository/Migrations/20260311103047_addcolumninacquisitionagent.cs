using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addcolumninacquisitionagent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PercentSharing",
                table: "AcquisitionAgents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "XPINType",
                table: "AcquisitionAgents",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentSharing",
                table: "AcquisitionAgents");

            migrationBuilder.DropColumn(
                name: "XPINType",
                table: "AcquisitionAgents");
        }
    }
}
