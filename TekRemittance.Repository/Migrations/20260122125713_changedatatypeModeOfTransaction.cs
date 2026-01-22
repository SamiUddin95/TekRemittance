using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class changedatatypeModeOfTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Xpin",
                table: "RemittanceInfos",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModeOfTransaction",
                table: "RemittanceInfos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LimitType",
                table: "RemittanceInfos",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DataJson",
                table: "RemittanceInfos",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AccountTitle",
                table: "RemittanceInfos",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "RemittanceInfos",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "barGraphDtos",
                columns: table => new
                {
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Complete_Count = table.Column<int>(type: "int", nullable: false),
                    Process_Count = table.Column<int>(type: "int", nullable: false),
                    Cancelled_Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_RemittanceInfos_AccountNumber",
                table: "RemittanceInfos",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_RemittanceInfos_AccountTitle",
                table: "RemittanceInfos",
                column: "AccountTitle");

            migrationBuilder.CreateIndex(
                name: "IX_RemittanceInfos_DataJson",
                table: "RemittanceInfos",
                column: "DataJson");

            migrationBuilder.CreateIndex(
                name: "IX_RemittanceInfos_Date",
                table: "RemittanceInfos",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_RemittanceInfos_LimitType",
                table: "RemittanceInfos",
                column: "LimitType");

            migrationBuilder.CreateIndex(
                name: "IX_RemittanceInfos_Xpin",
                table: "RemittanceInfos",
                column: "Xpin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "barGraphDtos");

            migrationBuilder.DropIndex(
                name: "IX_RemittanceInfos_AccountNumber",
                table: "RemittanceInfos");

            migrationBuilder.DropIndex(
                name: "IX_RemittanceInfos_AccountTitle",
                table: "RemittanceInfos");

            migrationBuilder.DropIndex(
                name: "IX_RemittanceInfos_DataJson",
                table: "RemittanceInfos");

            migrationBuilder.DropIndex(
                name: "IX_RemittanceInfos_Date",
                table: "RemittanceInfos");

            migrationBuilder.DropIndex(
                name: "IX_RemittanceInfos_LimitType",
                table: "RemittanceInfos");

            migrationBuilder.DropIndex(
                name: "IX_RemittanceInfos_Xpin",
                table: "RemittanceInfos");

            migrationBuilder.AlterColumn<string>(
                name: "Xpin",
                table: "RemittanceInfos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModeOfTransaction",
                table: "RemittanceInfos",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LimitType",
                table: "RemittanceInfos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DataJson",
                table: "RemittanceInfos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AccountTitle",
                table: "RemittanceInfos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "RemittanceInfos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
