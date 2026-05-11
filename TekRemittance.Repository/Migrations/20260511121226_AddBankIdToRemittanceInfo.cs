using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddBankIdToRemittanceInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RemittanceInfos_Banks_BankCode",
                table: "RemittanceInfos");

            migrationBuilder.DropIndex(
                name: "IX_RemittanceInfos_BankCode",
                table: "RemittanceInfos");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Banks_BankCode",
                table: "Banks");

            migrationBuilder.DropColumn(
                name: "BankCode",
                table: "RemittanceInfos");

            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "RemittanceInfos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RemittanceInfos_BankId",
                table: "RemittanceInfos",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_RemittanceInfos_Banks_BankId",
                table: "RemittanceInfos",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RemittanceInfos_Banks_BankId",
                table: "RemittanceInfos");

            migrationBuilder.DropIndex(
                name: "IX_RemittanceInfos_BankId",
                table: "RemittanceInfos");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "RemittanceInfos");

            migrationBuilder.AddColumn<string>(
                name: "BankCode",
                table: "RemittanceInfos",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Banks_BankCode",
                table: "Banks",
                column: "BankCode");

            migrationBuilder.CreateIndex(
                name: "IX_RemittanceInfos_BankCode",
                table: "RemittanceInfos",
                column: "BankCode");

            migrationBuilder.AddForeignKey(
                name: "FK_RemittanceInfos_Banks_BankCode",
                table: "RemittanceInfos",
                column: "BankCode",
                principalTable: "Banks",
                principalColumn: "BankCode");
        }
    }
}
