using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace MedicineInventory.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicineBatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicineBatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    BatchNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ManufacturingDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    SupplierName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineBatches_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineBatches_BatchNumber",
                table: "MedicineBatches",
                column: "BatchNumber");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineBatches_MedicineId",
                table: "MedicineBatches",
                column: "MedicineId");


            migrationBuilder.Sql(@"
            INSERT INTO MedicineBatches
              (MedicineId, BatchNumber, Quantity, ManufacturingDate,
              ExpiryDate, PurchasePrice, SellingPrice, SupplierName)
            SELECT
              Id, BatchNumber, Quantity, ManufacturingDate,
              ExpiryDate, PurchasePrice, SellingPrice, SupplierName
              FROM Medicines;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicineBatches");
        }
    }
}
