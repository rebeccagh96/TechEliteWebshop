using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechElite.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumThreads_ForumCategory_ForumCategoryId",
                table: "ForumThreads");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Orders_OrderId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductDepartment_ProductDepartmentId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_ForumCategory_ForumCategoryId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrderId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDepartment",
                table: "ProductDepartment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumCategory",
                table: "ForumCategory");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "ProductDepartment",
                newName: "ProductDepartments");

            migrationBuilder.RenameTable(
                name: "ForumCategory",
                newName: "ForumCategories");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Customers",
                newName: "LastName");

            migrationBuilder.AddColumn<byte>(
                name: "Image",
                table: "Products",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<bool>(
                name: "Delivered",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDepartments",
                table: "ProductDepartments",
                column: "ProductDepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumCategories",
                table: "ForumCategories",
                column: "ForumCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumThreads_ForumCategories_ForumCategoryId",
                table: "ForumThreads",
                column: "ForumCategoryId",
                principalTable: "ForumCategories",
                principalColumn: "ForumCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductDepartments_ProductDepartmentId",
                table: "Products",
                column: "ProductDepartmentId",
                principalTable: "ProductDepartments",
                principalColumn: "ProductDepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_ForumCategories_ForumCategoryId",
                table: "Replies",
                column: "ForumCategoryId",
                principalTable: "ForumCategories",
                principalColumn: "ForumCategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumThreads_ForumCategories_ForumCategoryId",
                table: "ForumThreads");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductDepartments_ProductDepartmentId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_ForumCategories_ForumCategoryId",
                table: "Replies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDepartments",
                table: "ProductDepartments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumCategories",
                table: "ForumCategories");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Delivered",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "ProductDepartments",
                newName: "ProductDepartment");

            migrationBuilder.RenameTable(
                name: "ForumCategories",
                newName: "ForumCategory");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Customers",
                newName: "Email");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDepartment",
                table: "ProductDepartment",
                column: "ProductDepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumCategory",
                table: "ForumCategory",
                column: "ForumCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderId",
                table: "Products",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumThreads_ForumCategory_ForumCategoryId",
                table: "ForumThreads",
                column: "ForumCategoryId",
                principalTable: "ForumCategory",
                principalColumn: "ForumCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Orders_OrderId",
                table: "Products",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductDepartment_ProductDepartmentId",
                table: "Products",
                column: "ProductDepartmentId",
                principalTable: "ProductDepartment",
                principalColumn: "ProductDepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_ForumCategory_ForumCategoryId",
                table: "Replies",
                column: "ForumCategoryId",
                principalTable: "ForumCategory",
                principalColumn: "ForumCategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
