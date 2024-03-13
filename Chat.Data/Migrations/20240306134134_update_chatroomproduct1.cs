using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chat.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_chatroomproduct1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomProduct_ChatRooms_ChatRoomId",
                table: "ChatRoomProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomProduct_Products_ProductId",
                table: "ChatRoomProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatRoomProduct",
                table: "ChatRoomProduct");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e74976ab-8639-44b0-96aa-35a874203838");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f645aebe-dc2f-4cab-8662-c9bbdec319e8");

            migrationBuilder.RenameTable(
                name: "ChatRoomProduct",
                newName: "ChatRoomProducts");

            migrationBuilder.RenameIndex(
                name: "IX_ChatRoomProduct_ProductId",
                table: "ChatRoomProducts",
                newName: "IX_ChatRoomProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatRoomProduct_ChatRoomId",
                table: "ChatRoomProducts",
                newName: "IX_ChatRoomProducts_ChatRoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatRoomProducts",
                table: "ChatRoomProducts",
                column: "ChatRoomProductId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "69c2abb4-3227-4fcb-a63b-14b4f505e02f", "1", "Admin", "Admin" },
                    { "c06f4997-08ee-486c-b2fc-fda7923b33b5", "2", "User", "User" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomProducts_ChatRooms_ChatRoomId",
                table: "ChatRoomProducts",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "ChatRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomProducts_Products_ProductId",
                table: "ChatRoomProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomProducts_ChatRooms_ChatRoomId",
                table: "ChatRoomProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomProducts_Products_ProductId",
                table: "ChatRoomProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatRoomProducts",
                table: "ChatRoomProducts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69c2abb4-3227-4fcb-a63b-14b4f505e02f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c06f4997-08ee-486c-b2fc-fda7923b33b5");

            migrationBuilder.RenameTable(
                name: "ChatRoomProducts",
                newName: "ChatRoomProduct");

            migrationBuilder.RenameIndex(
                name: "IX_ChatRoomProducts_ProductId",
                table: "ChatRoomProduct",
                newName: "IX_ChatRoomProduct_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatRoomProducts_ChatRoomId",
                table: "ChatRoomProduct",
                newName: "IX_ChatRoomProduct_ChatRoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatRoomProduct",
                table: "ChatRoomProduct",
                column: "ChatRoomProductId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "e74976ab-8639-44b0-96aa-35a874203838", "1", "Admin", "Admin" },
                    { "f645aebe-dc2f-4cab-8662-c9bbdec319e8", "2", "User", "User" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomProduct_ChatRooms_ChatRoomId",
                table: "ChatRoomProduct",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "ChatRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomProduct_Products_ProductId",
                table: "ChatRoomProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}
