using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chat.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_cascade_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomProducts_Products_ProductId",
                table: "ChatRoomProducts");

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
                    { "4d3cf7bb-8335-4691-bc83-63a421b14324", "2", "User", "User" },
                    { "ba67ca5f-1f32-4c5b-8a73-07c476fe729e", "1", "Admin", "Admin" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomProducts_Products_ProductId",
                table: "ChatRoomProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomProducts_Products_ProductId",
                table: "ChatRoomProducts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d3cf7bb-8335-4691-bc83-63a421b14324");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba67ca5f-1f32-4c5b-8a73-07c476fe729e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "58e3e1ef-075a-41d9-b97d-05ec422bd4ef", "2", "User", "User" },
                    { "86ed554a-66e3-49c3-a024-1461eea1b465", "1", "Admin", "Admin" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomProducts_Products_ProductId",
                table: "ChatRoomProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}
