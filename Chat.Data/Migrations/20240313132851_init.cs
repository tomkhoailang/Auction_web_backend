using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chat.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69c2abb4-3227-4fcb-a63b-14b4f505e02f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c06f4997-08ee-486c-b2fc-fda7923b33b5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2a39d34e-0e1a-4e13-9dc2-390f59b3553b", "2", "User", "User" },
                    { "876c7c74-92c3-4b74-a1e4-362d35687c15", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                    { "69c2abb4-3227-4fcb-a63b-14b4f505e02f", "1", "Admin", "Admin" },
                    { "c06f4997-08ee-486c-b2fc-fda7923b33b5", "2", "User", "User" }
                });
        }
    }
}
