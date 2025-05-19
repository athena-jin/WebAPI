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
                    { new Guid("627acde5-94af-49a5-9a96-e072e737e4ea"), "192.168.1.10", 0, "Machine A", 8080u, 0 },
                    { new Guid("65484d17-708e-4794-93eb-9636932ab5c9"), "192.168.1.11", 0, "Machine B", 8081u, 2 },
                    { new Guid("9d3d94b2-e5cc-4b97-8253-0e455197611a"), "opc.tcp://localhost", 2, "Video One", 4840u, 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Password" },
                values: new object[,]
                {
                    { new Guid("094c07d2-64d5-4bde-bc8a-14d688bee080"), "Default", "Default" },
                    { new Guid("c5d62bfb-865f-4393-a601-103f9c522139"), "admin", "admin" }
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
