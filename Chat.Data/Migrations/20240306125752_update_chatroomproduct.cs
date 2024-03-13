using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chat.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_chatroomproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ChatRooms_ChatRoomId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ChatRoomId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09fd845d-a43d-4dcc-acb6-694d852a044a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "54eb11c2-c31f-4a46-8ee8-0ce1decf4873");

            migrationBuilder.DropColumn(
                name: "BiddingEndTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BiddingStartTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ChatRoomId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ChatRoomProduct",
                columns: table => new
                {
                    ChatRoomProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatRoomId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    BiddingStartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BiddingEndTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomProduct", x => x.ChatRoomProductId);
                    table.ForeignKey(
                        name: "FK_ChatRoomProduct_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "ChatRoomId");
                    table.ForeignKey(
                        name: "FK_ChatRoomProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "e74976ab-8639-44b0-96aa-35a874203838", "1", "Admin", "Admin" },
                    { "f645aebe-dc2f-4cab-8662-c9bbdec319e8", "2", "User", "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomProduct_ChatRoomId",
                table: "ChatRoomProduct",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomProduct_ProductId",
                table: "ChatRoomProduct",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatRoomProduct");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e74976ab-8639-44b0-96aa-35a874203838");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f645aebe-dc2f-4cab-8662-c9bbdec319e8");

            migrationBuilder.AddColumn<DateTime>(
                name: "BiddingEndTime",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BiddingStartTime",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChatRoomId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "09fd845d-a43d-4dcc-acb6-694d852a044a", "2", "User", "User" },
                    { "54eb11c2-c31f-4a46-8ee8-0ce1decf4873", "1", "Admin", "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ChatRoomId",
                table: "Products",
                column: "ChatRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ChatRooms_ChatRoomId",
                table: "Products",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "ChatRoomId");
        }
    }
}
