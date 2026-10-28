using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineInventory.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLegacyBatchColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Medicines_BatchNumber",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "BatchNumber",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "ManufacturingDate",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "SupplierName",
                table: "Medicines");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BatchNumber",
                table: "Medicines",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "Medicines",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ManufacturingDate",
                table: "Medicines",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "Medicines",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Medicines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                table: "Medicines",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SupplierName",
                table: "Medicines",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_BatchNumber",
                table: "Medicines",
                column: "BatchNumber");
        }
    }
}
