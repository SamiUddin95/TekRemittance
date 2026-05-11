using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BankCode",
                table: "Banks",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "BankCode",
                table: "Banks",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3);
        }
    }
}
