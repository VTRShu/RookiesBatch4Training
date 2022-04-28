using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore_Ex2.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryEntity",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryEntity", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ProductEntity",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manufacture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEntity", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_ProductEntity_CategoryEntity_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryEntity",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "CategoryEntity",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[] { new Guid("df44bc42-6785-459b-8185-4d7f7c16b544"), "Laptop" });

            migrationBuilder.InsertData(
                table: "ProductEntity",
                columns: new[] { "ProductId", "CategoryId", "Manufacture", "ProductName" },
                values: new object[] { new Guid("614d1d14-5100-4e79-ba17-e0d6db051de8"), new Guid("df44bc42-6785-459b-8185-4d7f7c16b544"), "Dell", "Alienware X17" });

            migrationBuilder.InsertData(
                table: "ProductEntity",
                columns: new[] { "ProductId", "CategoryId", "Manufacture", "ProductName" },
                values: new object[] { new Guid("c6fd585d-2227-43d8-b1a2-e5260eeb58ad"), new Guid("df44bc42-6785-459b-8185-4d7f7c16b544"), "Dell", "Alienware 15R6" });

            migrationBuilder.InsertData(
                table: "ProductEntity",
                columns: new[] { "ProductId", "CategoryId", "Manufacture", "ProductName" },
                values: new object[] { new Guid("dd07a59f-e8ef-459e-8a17-ffd43476a9b8"), new Guid("df44bc42-6785-459b-8185-4d7f7c16b544"), "MSI", "MSI modern 14 B11" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductEntity_CategoryId",
                table: "ProductEntity",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductEntity");

            migrationBuilder.DropTable(
                name: "CategoryEntity");
        }
    }
}
