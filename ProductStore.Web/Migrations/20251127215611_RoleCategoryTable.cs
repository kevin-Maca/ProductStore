using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductStore.Web.Migrations
{
    /// <inheritdoc />
    public partial class RoleCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleCategories",
                columns: table => new
                {
                    ProductStoreRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleCategories", x => new { x.CategoryId, x.ProductStoreRoleId });
                    table.ForeignKey(
                        name: "FK_RoleCategories_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleCategories_ProductStoreRole_ProductStoreRoleId",
                        column: x => x.ProductStoreRoleId,
                        principalTable: "ProductStoreRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleCategories_ProductStoreRoleId",
                table: "RoleCategories",
                column: "ProductStoreRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleCategories");
        }
    }
}
