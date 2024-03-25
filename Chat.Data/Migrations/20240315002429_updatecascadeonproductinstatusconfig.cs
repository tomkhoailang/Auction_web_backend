using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chat.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatecascadeonproductinstatusconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInStatus_Products_ProductId",
                table: "ProductInStatus");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a39d34e-0e1a-4e13-9dc2-390f59b3553b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "876c7c74-92c3-4b74-a1e4-362d35687c15");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "58e3e1ef-075a-41d9-b97d-05ec422bd4ef", "2", "User", "User" },
                    { "86ed554a-66e3-49c3-a024-1461eea1b465", "1", "Admin", "Admin" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInStatus_Products_ProductId",
                table: "ProductInStatus",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInStatus_Products_ProductId",
                table: "ProductInStatus");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58e3e1ef-075a-41d9-b97d-05ec422bd4ef");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "86ed554a-66e3-49c3-a024-1461eea1b465");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2a39d34e-0e1a-4e13-9dc2-390f59b3553b", "2", "User", "User" },
                    { "876c7c74-92c3-4b74-a1e4-362d35687c15", "1", "Admin", "Admin" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInStatus_Products_ProductId",
                table: "ProductInStatus",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}
