using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechElite.Data.Migrations
{
    /// <inheritdoc />
    public partial class seventh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductId1_ProductDepartmentId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ProductId1_ProductDepartmentId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId_ProductDepartmentId",
                table: "Reviews",
                columns: new[] { "ProductId", "ProductDepartmentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductId_ProductDepartmentId",
                table: "Reviews",
                columns: new[] { "ProductId", "ProductDepartmentId" },
                principalTable: "Products",
                principalColumns: new[] { "ProductId", "ProductDepartmentId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductId_ProductDepartmentId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ProductId_ProductDepartmentId",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId1_ProductDepartmentId",
                table: "Reviews",
                columns: new[] { "ProductId1", "ProductDepartmentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductId1_ProductDepartmentId",
                table: "Reviews",
                columns: new[] { "ProductId1", "ProductDepartmentId" },
                principalTable: "Products",
                principalColumns: new[] { "ProductId", "ProductDepartmentId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
