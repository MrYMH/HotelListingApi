using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListingApi.EF.Migrations
{
    /// <inheritdoc />
    public partial class setRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "06815283-050b-411c-b1fd-96d88dd5f838", null, "Administrator", "ADMINISTRATOR" },
                    { "a00bbbe6-b312-4548-9f4b-e2bf6f4faa5b", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06815283-050b-411c-b1fd-96d88dd5f838");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a00bbbe6-b312-4548-9f4b-e2bf6f4faa5b");
        }
    }
}
