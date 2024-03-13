using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chat.Data.Migrations
{
    /// <inheritdoc />
    public partial class addTimeToChatRoomAndProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2917e260-3804-4d8a-be90-8b5958bf0194");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5135dd8b-0ef9-46f2-9b2a-a88e9ecb04ac");

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

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ChatRooms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "ChatRooms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "09fd845d-a43d-4dcc-acb6-694d852a044a", "2", "User", "User" },
                    { "54eb11c2-c31f-4a46-8ee8-0ce1decf4873", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "EndDate",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ChatRooms");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2917e260-3804-4d8a-be90-8b5958bf0194", "1", "Admin", "Admin" },
                    { "5135dd8b-0ef9-46f2-9b2a-a88e9ecb04ac", "2", "User", "User" }
                });
        }
    }
}
