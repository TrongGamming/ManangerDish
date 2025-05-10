using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagerDish.Migrations
{
    public partial class updateGuestAndOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Guests",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 10, 0, 52, 2, 488, DateTimeKind.Local).AddTicks(9818), "$2a$11$PdbBsIBrRzaWpeQND6sCReUoqliIO/FKJtUdBF6w9dBIphxij99Fi", new DateTime(2025, 5, 10, 0, 52, 2, 488, DateTimeKind.Local).AddTicks(9827) });

            migrationBuilder.CreateIndex(
                name: "IX_Guests_OrderId",
                table: "Guests",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_Orders_OrderId",
                table: "Guests",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_Orders_OrderId",
                table: "Guests");

            migrationBuilder.DropIndex(
                name: "IX_Guests_OrderId",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Guests");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 53, 44, 553, DateTimeKind.Local).AddTicks(6874), "$2a$11$Ve8MKkHv7SBYDsyq2vJkvuZVXFbAlvzfWfOWazES11g4H0k7QLJ7a", new DateTime(2025, 5, 9, 15, 53, 44, 553, DateTimeKind.Local).AddTicks(6883) });
        }
    }
}
