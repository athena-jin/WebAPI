using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    BirthTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    Port = table.Column<uint>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ActorId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_Actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "BirthTime", "Name" },
                values: new object[,]
                {
                    { new Guid("b69d8e13-a56e-41d2-af7a-3628497ce01d"), new DateTime(1990, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Actor Two" },
                    { new Guid("bcfa83f9-435e-4903-a9a0-41a1056f91bd"), new DateTime(1985, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Actor One" }
                });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "Address", "Name", "Port", "Status" },
                values: new object[,]
                {
                    { new Guid("4bb8e41e-26db-4257-af34-621989e7ebd2"), "192.168.1.11", "Machine B", 8081u, 2 },
                    { new Guid("b65af4e8-8ff7-4dd1-9f95-6078e27c9e42"), "192.168.1.10", "Machine A", 8080u, 0 }
                });

            migrationBuilder.InsertData(
                table: "Videos",
                columns: new[] { "Id", "ActorId", "Name" },
                values: new object[,]
                {
                    { new Guid("2225e305-a5ff-468b-bf9e-8cb1344cf3df"), null, "Video Two" },
                    { new Guid("aaf5c9cd-52bd-4583-b245-f1cabb1bdb06"), null, "Video One" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Videos_ActorId",
                table: "Videos",
                column: "ActorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Machines");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "Actors");
        }
    }
}
