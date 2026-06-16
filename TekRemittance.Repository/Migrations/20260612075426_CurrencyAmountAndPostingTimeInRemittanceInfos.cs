using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyAmountAndPostingTimeInRemittanceInfos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ForeignCurrencyAmount",
                table: "RemittanceInfos",
                type: "decimal(4,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostingTime",
                table: "RemittanceInfos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForeignCurrencyAmount",
                table: "RemittanceInfos");

            migrationBuilder.DropColumn(
                name: "PostingTime",
                table: "RemittanceInfos");
        }
    }
}
