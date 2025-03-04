using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LUXLocks_projekt.Data.Migrations
{
    /// <inheritdoc />
    public partial class AllControllersAndViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    HairLength = table.Column<string>(type: "TEXT", nullable: false),
                    HairType = table.Column<string>(type: "TEXT", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stylists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Bio = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stylists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppointmentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    StylistModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    TreatmentModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    HairLength = table.Column<string>(type: "TEXT", nullable: true),
                    HairType = table.Column<string>(type: "TEXT", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "TEXT", nullable: true),
                    SilentTreatment = table.Column<bool>(type: "INTEGER", nullable: false),
                    BookedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Stylists_StylistModelId",
                        column: x => x.StylistModelId,
                        principalTable: "Stylists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Treatments_TreatmentModelId",
                        column: x => x.TreatmentModelId,
                        principalTable: "Treatments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TreatmentModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Treatments_TreatmentModelId",
                        column: x => x.TreatmentModelId,
                        principalTable: "Treatments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_StylistModelId",
                table: "Appointments",
                column: "StylistModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TreatmentModelId",
                table: "Appointments",
                column: "TreatmentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfiles_UserId",
                table: "CustomerProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_TreatmentModelId",
                table: "Reviews",
                column: "TreatmentModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "CustomerProfiles");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Stylists");

            migrationBuilder.DropTable(
                name: "Treatments");
        }
    }
}
