using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagerDish.Migrations
{
    public partial class updateRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 5, 23, 3, 46, 932, DateTimeKind.Local).AddTicks(5052), "$2a$11$jDjU.UP3GmBihA8wqmMuoeJEmv/cUvAqMx2Ig50IuvxXOCrm4qV/.", new DateTime(2025, 5, 5, 23, 3, 46, 932, DateTimeKind.Local).AddTicks(5065) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 5, 22, 28, 22, 820, DateTimeKind.Local).AddTicks(418), "$2a$11$AlbPhjLJKJTEwpWxBmZPtOkHHsKFn2tZCt1sDreDwxYaS0LknhZuW", new DateTime(2025, 5, 5, 22, 28, 22, 820, DateTimeKind.Local).AddTicks(429) });
        }
    }
}
