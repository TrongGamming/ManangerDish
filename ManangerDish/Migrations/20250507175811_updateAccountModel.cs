using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagerDish.Migrations
{
    public partial class updateAccountModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 8, 0, 58, 11, 217, DateTimeKind.Local).AddTicks(3487), "$2a$11$uCBApWEP7.zfH2F7My0e6.a6yY3aMiHozIZShYzYVJYk7LWoS7kxu", new DateTime(2025, 5, 8, 0, 58, 11, 217, DateTimeKind.Local).AddTicks(3499) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 8, 0, 50, 19, 588, DateTimeKind.Local).AddTicks(3353), "$2a$11$xqgqaapVhLRnt9exvodLmuSu1eZZUWwHxiogZjX2FNE0/e8oCrLv2", new DateTime(2025, 5, 8, 0, 50, 19, 588, DateTimeKind.Local).AddTicks(3362) });
        }
    }
}
