using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TechElite.Migrations
{
    /// <inheritdoc />
    public partial class seedingupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "City", "FirstName", "LastName", "UserId", "ZipCode" },
                values: new object[,]
                {
                    { 1, "User street 1", "User city", "User1", "Userson", "0db21edc-bccd-41e6-b80f-6d1c769dd7a7", "12345" },
                    { 2, "User street 2", "User city", "User2", "Userson2", "0bea84fb-9909-4cb7-8a5e-9db0ca44b4f4", "54321" }
                });

            migrationBuilder.InsertData(
                table: "ForumThreads",
                columns: new[] { "ForumThreadId", "ForumCategoryId", "PublishDate", "ThreadContent", "ThreadTitle", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 3, 31, 12, 0, 0, 0, DateTimeKind.Unspecified), "Vilken dator är bäst?", "Bästa datorn?", "4e031e7b-fd2a-47a8-b8a3-88e3f1c7f38d" },
                    { 2, 2, new DateTime(2024, 3, 31, 12, 0, 0, 0, DateTimeKind.Unspecified), "Min iPhone fungerar inte", "Hjälp med iPhone", "0bea84fb-9909-4cb7-8a5e-9db0ca44b4f4" }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "ProductId", "Rating", "ReviewContent", "UserId" },
                values: new object[,]
                {
                    { 1, 1, 5, "Bra dator!", "0db21edc-bccd-41e6-b80f-6d1c769dd7a7" },
                    { 2, 2, 1, "Dålig dator!", "4e031e7b-fd2a-47a8-b8a3-88e3f1c7f38d" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ForumThreads",
                keyColumn: "ForumThreadId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ForumThreads",
                keyColumn: "ForumThreadId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 2);
        }
    }
}
