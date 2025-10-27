using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RenameProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "AcquisitionAgentAccounts");

            migrationBuilder.RenameColumn(
                name: "AcquisitionAgentName",
                table: "AcquisitionAgentAccounts",
                newName: "AgentName");

            migrationBuilder.RenameColumn(
                name: "AcquisitionAgentAccountName",
                table: "AcquisitionAgentAccounts",
                newName: "AgentAccountName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AgentName",
                table: "AcquisitionAgentAccounts",
                newName: "AcquisitionAgentName");

            migrationBuilder.RenameColumn(
                name: "AgentAccountName",
                table: "AcquisitionAgentAccounts",
                newName: "AcquisitionAgentAccountName");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "AcquisitionAgentAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
