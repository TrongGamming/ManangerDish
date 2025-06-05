using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagerDish.Migrations
{
    public partial class addColumnForOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 2, 23, 30, 23, 500, DateTimeKind.Local).AddTicks(4086), "$2a$11$BlTb8W22qkaJAYyb6JBc9.EWqp0j/q/qr5qYMd5/TUJ72mYzdOaoK", new DateTime(2025, 6, 2, 23, 30, 23, 500, DateTimeKind.Local).AddTicks(4105) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 28, 21, 14, 31, 73, DateTimeKind.Local).AddTicks(117), "$2a$11$hNQLSX5vLiEux7N.FabsPOz8GBqtXgTeoif1Xm5IUJQzTLMPzTn7S", new DateTime(2025, 5, 28, 21, 14, 31, 73, DateTimeKind.Local).AddTicks(127) });
        }
    }
}
