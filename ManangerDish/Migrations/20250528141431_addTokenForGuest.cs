using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagerDish.Migrations
{
    public partial class addTokenForGuest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 28, 21, 14, 31, 73, DateTimeKind.Local).AddTicks(117), "$2a$11$hNQLSX5vLiEux7N.FabsPOz8GBqtXgTeoif1Xm5IUJQzTLMPzTn7S", new DateTime(2025, 5, 28, 21, 14, 31, 73, DateTimeKind.Local).AddTicks(127) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Guests");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 10, 0, 52, 2, 488, DateTimeKind.Local).AddTicks(9818), "$2a$11$PdbBsIBrRzaWpeQND6sCReUoqliIO/FKJtUdBF6w9dBIphxij99Fi", new DateTime(2025, 5, 10, 0, 52, 2, 488, DateTimeKind.Local).AddTicks(9827) });
        }
    }
}
