using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekRemittance.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AcquisitionAgent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcquisitionAgents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AgentName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone1 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Phone2 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProvinceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CutOffTimeStart = table.Column<TimeSpan>(type: "time", nullable: false),
                    CutOffTimeEnd = table.Column<TimeSpan>(type: "time", nullable: false),
                    RIN = table.Column<int>(type: "int", nullable: false),
                    Process = table.Column<int>(type: "int", nullable: false),
                    AcquisitionModes = table.Column<int>(type: "int", nullable: false),
                    DisbursementModes = table.Column<int>(type: "int", nullable: false),
                    DirectIntegration = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    InquiryURL = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    PaymentURL = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    UnlockURL = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcquisitionAgents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcquisitionAgents_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcquisitionAgents_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcquisitionAgents_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcquisitionAgents_CityId",
                table: "AcquisitionAgents",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AcquisitionAgents_Code",
                table: "AcquisitionAgents",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcquisitionAgents_CountryId",
                table: "AcquisitionAgents",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AcquisitionAgents_ProvinceId",
                table: "AcquisitionAgents",
                column: "ProvinceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcquisitionAgents");
        }
    }
}
