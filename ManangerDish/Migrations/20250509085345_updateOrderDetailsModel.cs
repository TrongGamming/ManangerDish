using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagerDish.Migrations
{
    public partial class updateOrderDetailsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Accounts_HandlerId",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "HandlerId",
                table: "OrderDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 53, 44, 553, DateTimeKind.Local).AddTicks(6874), "$2a$11$Ve8MKkHv7SBYDsyq2vJkvuZVXFbAlvzfWfOWazES11g4H0k7QLJ7a", new DateTime(2025, 5, 9, 15, 53, 44, 553, DateTimeKind.Local).AddTicks(6883) });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Accounts_HandlerId",
                table: "OrderDetails",
                column: "HandlerId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Accounts_HandlerId",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "HandlerId",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 8, 15, 33, 27, 948, DateTimeKind.Local).AddTicks(1764), "$2a$11$yIlvqLv2G.gq422DyKUeMOERSrNz.0WzvCGzWEt5xAkIP.FKFh/Fu", new DateTime(2025, 5, 8, 15, 33, 27, 948, DateTimeKind.Local).AddTicks(1772) });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Accounts_HandlerId",
                table: "OrderDetails",
                column: "HandlerId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
