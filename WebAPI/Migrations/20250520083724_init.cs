using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Port = table.Column<uint>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ConnectorType = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarningRecordDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MachineId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WarningLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarningRecordDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarningRecordDetails_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarningRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MachineId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WarningLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarningRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarningRecords_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "Address", "ConnectorType", "Name", "Port", "Status" },
                values: new object[,]
                {
                    { new Guid("7a5e700a-45b9-4320-80a4-48e0af766bfa"), "opc.tcp://localhost", 2, "Video One", 4840u, 0 },
                    { new Guid("e78e2c10-1679-4de9-ac77-35eb9c127803"), "192.168.1.11", 0, "Machine B", 8081u, 2 },
                    { new Guid("eee48de2-5afd-481d-9f4d-c5b9236f48f6"), "192.168.1.10", 0, "Machine A", 8080u, 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Password" },
                values: new object[,]
                {
                    { new Guid("690545c7-f0a5-494c-94c5-9aa9c4dc2854"), "Default", "Default" },
                    { new Guid("ddee17be-bbcd-4ded-8cf2-f2123c05770c"), "admin", "admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarningRecordDetails_MachineId",
                table: "WarningRecordDetails",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_WarningRecords_MachineId",
                table: "WarningRecords",
                column: "MachineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WarningRecordDetails");

            migrationBuilder.DropTable(
                name: "WarningRecords");

            migrationBuilder.DropTable(
                name: "Machines");
        }
    }
}
