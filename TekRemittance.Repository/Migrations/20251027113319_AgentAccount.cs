using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AgentAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcquisitionAgentAccounts");

            migrationBuilder.CreateTable(
                name: "AgentAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgentAccountName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AccountNumber = table.Column<int>(type: "int", nullable: false),
                    AgentName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Approve = table.Column<bool>(type: "bit", nullable: false),
                    AccountTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentAccounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentAccounts_AgentAccountName",
                table: "AgentAccounts",
                column: "AgentAccountName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentAccounts");

            migrationBuilder.CreateTable(
                name: "AcquisitionAgentAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumber = table.Column<int>(type: "int", nullable: false),
                    AccountTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AgentAccountName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AgentName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Approve = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcquisitionAgentAccounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcquisitionAgentAccounts_AgentAccountName",
                table: "AcquisitionAgentAccounts",
                column: "AgentAccountName",
                unique: true);
        }
    }
}
