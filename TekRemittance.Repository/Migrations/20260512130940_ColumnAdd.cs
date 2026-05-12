using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ColumnAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "AcquisitionAgents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartIndex",
                table: "AcquisitionAgents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "XPinMapCode",
                table: "AcquisitionAgents",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Length",
                table: "AcquisitionAgents");

            migrationBuilder.DropColumn(
                name: "StartIndex",
                table: "AcquisitionAgents");

            migrationBuilder.DropColumn(
                name: "XPinMapCode",
                table: "AcquisitionAgents");
        }
    }
}
