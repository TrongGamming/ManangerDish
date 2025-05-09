using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagerDish.Migrations
{
    public partial class updateCategoryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Categories_CategoryId",
                table: "Dishes");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Dishes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 8, 15, 33, 27, 948, DateTimeKind.Local).AddTicks(1764), "$2a$11$yIlvqLv2G.gq422DyKUeMOERSrNz.0WzvCGzWEt5xAkIP.FKFh/Fu", new DateTime(2025, 5, 8, 15, 33, 27, 948, DateTimeKind.Local).AddTicks(1772) });

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Categories_CategoryId",
                table: "Dishes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Categories_CategoryId",
                table: "Dishes");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Dishes",
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
                values: new object[] { new DateTime(2025, 5, 8, 0, 58, 11, 217, DateTimeKind.Local).AddTicks(3487), "$2a$11$uCBApWEP7.zfH2F7My0e6.a6yY3aMiHozIZShYzYVJYk7LWoS7kxu", new DateTime(2025, 5, 8, 0, 58, 11, 217, DateTimeKind.Local).AddTicks(3499) });

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Categories_CategoryId",
                table: "Dishes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
