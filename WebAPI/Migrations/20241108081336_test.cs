using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: new Guid("b69d8e13-a56e-41d2-af7a-3628497ce01d"));

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: new Guid("bcfa83f9-435e-4903-a9a0-41a1056f91bd"));

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "Id",
                keyValue: new Guid("4bb8e41e-26db-4257-af34-621989e7ebd2"));

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "Id",
                keyValue: new Guid("b65af4e8-8ff7-4dd1-9f95-6078e27c9e42"));

            migrationBuilder.DeleteData(
                table: "Videos",
                keyColumn: "Id",
                keyValue: new Guid("2225e305-a5ff-468b-bf9e-8cb1344cf3df"));

            migrationBuilder.DeleteData(
                table: "Videos",
                keyColumn: "Id",
                keyValue: new Guid("aaf5c9cd-52bd-4583-b245-f1cabb1bdb06"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Videos",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Machines",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConnectorType",
                table: "Machines",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "BirthTime", "Name" },
                values: new object[,]
                {
                    { new Guid("22377d34-8fe0-4feb-b3f7-d58cd0a44196"), new DateTime(1990, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Actor Two" },
                    { new Guid("c4e2e6dc-ce2f-424d-a5c4-d7b1c64f9a2b"), new DateTime(1985, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Actor One" }
                });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "Address", "ConnectorType", "Name", "Port", "Status" },
                values: new object[,]
                {
                    { new Guid("7c822da6-330c-4948-8261-894f9658b0cd"), "opc.tcp://localhost", 2, "Video One", 4840u, 0 },
                    { new Guid("9be31da9-e66e-48d2-a613-dc0aa7757aab"), "192.168.1.10", 0, "Machine A", 8080u, 0 },
                    { new Guid("f49b9b89-3372-4852-8e8d-e3a8905671dd"), "192.168.1.11", 0, "Machine B", 8081u, 2 }
                });

            migrationBuilder.InsertData(
                table: "Videos",
                columns: new[] { "Id", "ActorId", "Name" },
                values: new object[,]
                {
                    { new Guid("1564043c-4b5d-46d4-9d1d-09fba4b73dc0"), null, "Video One" },
                    { new Guid("5433e5f8-dac4-4948-8cb7-99921d22adf0"), null, "Video Two" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: new Guid("22377d34-8fe0-4feb-b3f7-d58cd0a44196"));

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: new Guid("c4e2e6dc-ce2f-424d-a5c4-d7b1c64f9a2b"));

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "Id",
                keyValue: new Guid("7c822da6-330c-4948-8261-894f9658b0cd"));

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "Id",
                keyValue: new Guid("9be31da9-e66e-48d2-a613-dc0aa7757aab"));

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "Id",
                keyValue: new Guid("f49b9b89-3372-4852-8e8d-e3a8905671dd"));

            migrationBuilder.DeleteData(
                table: "Videos",
                keyColumn: "Id",
                keyValue: new Guid("1564043c-4b5d-46d4-9d1d-09fba4b73dc0"));

            migrationBuilder.DeleteData(
                table: "Videos",
                keyColumn: "Id",
                keyValue: new Guid("5433e5f8-dac4-4948-8cb7-99921d22adf0"));

            migrationBuilder.DropColumn(
                name: "ConnectorType",
                table: "Machines");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Videos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Machines",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

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
        }
    }
}
