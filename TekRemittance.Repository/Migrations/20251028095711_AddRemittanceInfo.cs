using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddRemittanceInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RemittanceInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowNumber = table.Column<int>(type: "int", nullable: false),
                    DataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Error = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemittanceInfos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RemittanceInfos_AgentId",
                table: "RemittanceInfos",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_RemittanceInfos_TemplateId",
                table: "RemittanceInfos",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RemittanceInfos_UploadId",
                table: "RemittanceInfos",
                column: "UploadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RemittanceInfos");
        }
    }
}
