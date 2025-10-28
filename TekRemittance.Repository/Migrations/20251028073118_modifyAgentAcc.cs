using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class modifyAgentAcc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AgentAccounts_AgentAccountName",
                table: "AgentAccounts");

            migrationBuilder.DropColumn(
                name: "AgentAccountName",
                table: "AgentAccounts");

            migrationBuilder.DropColumn(
                name: "AgentName",
                table: "AgentAccounts");

            migrationBuilder.AddColumn<Guid>(
                name: "AgentId",
                table: "AgentAccounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AgentAccounts_AgentId",
                table: "AgentAccounts",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentAccounts_AcquisitionAgents_AgentId",
                table: "AgentAccounts",
                column: "AgentId",
                principalTable: "AcquisitionAgents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentAccounts_AcquisitionAgents_AgentId",
                table: "AgentAccounts");

            migrationBuilder.DropIndex(
                name: "IX_AgentAccounts_AgentId",
                table: "AgentAccounts");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "AgentAccounts");

            migrationBuilder.AddColumn<string>(
                name: "AgentAccountName",
                table: "AgentAccounts",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AgentName",
                table: "AgentAccounts",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AgentAccounts_AgentAccountName",
                table: "AgentAccounts",
                column: "AgentAccountName",
                unique: true);
        }
    }
}
