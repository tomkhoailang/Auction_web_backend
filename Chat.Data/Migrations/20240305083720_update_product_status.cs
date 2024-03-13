using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chat.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_product_status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bidding_AspNetUsers_BiddingUserId",
                table: "Bidding");

            migrationBuilder.DropForeignKey(
                name: "FK_Bidding_Product_ProductId",
                table: "Bidding");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_AspNetUsers_SellerId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ChatRooms_ChatRoomId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_Product_ProductId",
                table: "ProductImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImage",
                table: "ProductImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bidding",
                table: "Bidding");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ae8f12c5-f400-4d94-ac95-17adf194294d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c53d7638-2644-469a-a39f-b46c4a0682b4");

            migrationBuilder.RenameTable(
                name: "ProductImage",
                newName: "ProductImages");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "Bidding",
                newName: "Biddings");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImages",
                newName: "IX_ProductImages_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_SellerId",
                table: "Products",
                newName: "IX_Products_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ChatRoomId",
                table: "Products",
                newName: "IX_Products_ChatRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Bidding_ProductId",
                table: "Biddings",
                newName: "IX_Biddings_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Bidding_BiddingUserId",
                table: "Biddings",
                newName: "IX_Biddings_BiddingUserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Biddings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages",
                column: "ProductImageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Biddings",
                table: "Biddings",
                column: "BiddingId");

            migrationBuilder.CreateTable(
                name: "ProductStatuses",
                columns: table => new
                {
                    ProductStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductStatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStatuses", x => x.ProductStatusId);
                });

            migrationBuilder.CreateTable(
                name: "ProductInStatus",
                columns: table => new
                {
                    ProductInStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInStatus", x => x.ProductInStatusID);
                    table.ForeignKey(
                        name: "FK_ProductInStatus_ProductStatuses_ProductStatusId",
                        column: x => x.ProductStatusId,
                        principalTable: "ProductStatuses",
                        principalColumn: "ProductStatusId");
                    table.ForeignKey(
                        name: "FK_ProductInStatus_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2917e260-3804-4d8a-be90-8b5958bf0194", "1", "Admin", "Admin" },
                    { "5135dd8b-0ef9-46f2-9b2a-a88e9ecb04ac", "2", "User", "User" }
                });

            migrationBuilder.InsertData(
                table: "ProductStatuses",
                columns: new[] { "ProductStatusId", "ProductStatusName" },
                values: new object[,]
                {
                    { 1, "Waiting to accept" },
                    { 2, "Your product has been registered successfully" },
                    { 3, "Auction in Progress" },
                    { 4, "Sold" },
                    { 5, "Expired" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductInStatus_ProductId",
                table: "ProductInStatus",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInStatus_ProductStatusId",
                table: "ProductInStatus",
                column: "ProductStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Biddings_AspNetUsers_BiddingUserId",
                table: "Biddings",
                column: "BiddingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Biddings_Products_ProductId",
                table: "Biddings",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_SellerId",
                table: "Products",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ChatRooms_ChatRoomId",
                table: "Products",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "ChatRoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Biddings_AspNetUsers_BiddingUserId",
                table: "Biddings");

            migrationBuilder.DropForeignKey(
                name: "FK_Biddings_Products_ProductId",
                table: "Biddings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_SellerId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ChatRooms_ChatRoomId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductInStatus");

            migrationBuilder.DropTable(
                name: "ProductStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Biddings",
                table: "Biddings");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2917e260-3804-4d8a-be90-8b5958bf0194");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5135dd8b-0ef9-46f2-9b2a-a88e9ecb04ac");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Biddings");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "ProductImages",
                newName: "ProductImage");

            migrationBuilder.RenameTable(
                name: "Biddings",
                newName: "Bidding");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SellerId",
                table: "Product",
                newName: "IX_Product_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ChatRoomId",
                table: "Product",
                newName: "IX_Product_ChatRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImage",
                newName: "IX_ProductImage_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Biddings_ProductId",
                table: "Bidding",
                newName: "IX_Bidding_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Biddings_BiddingUserId",
                table: "Bidding",
                newName: "IX_Bidding_BiddingUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImage",
                table: "ProductImage",
                column: "ProductImageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bidding",
                table: "Bidding",
                column: "BiddingId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ae8f12c5-f400-4d94-ac95-17adf194294d", "2", "User", "User" },
                    { "c53d7638-2644-469a-a39f-b46c4a0682b4", "1", "Admin", "Admin" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bidding_AspNetUsers_BiddingUserId",
                table: "Bidding",
                column: "BiddingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bidding_Product_ProductId",
                table: "Bidding",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_SellerId",
                table: "Product",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ChatRooms_ChatRoomId",
                table: "Product",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "ChatRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImage_Product_ProductId",
                table: "ProductImage",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
