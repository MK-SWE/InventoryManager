using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeStockStatusOwnedPropertyOfProductStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductStockStatuses");

            migrationBuilder.RenameColumn(
                name: "ReturnedStock",
                table: "ProductStocks",
                newName: "StockStatus_ReturnedStock");

            migrationBuilder.RenameColumn(
                name: "QuarantinedStock",
                table: "ProductStocks",
                newName: "StockStatus_QuarantinedStock");

            migrationBuilder.RenameColumn(
                name: "QualityControlStock",
                table: "ProductStocks",
                newName: "StockStatus_QualityControlStock");

            migrationBuilder.RenameColumn(
                name: "OnHoldStock",
                table: "ProductStocks",
                newName: "StockStatus_OnHoldStock");

            migrationBuilder.RenameColumn(
                name: "DamagedStock",
                table: "ProductStocks",
                newName: "StockStatus_DamagedStock");

            migrationBuilder.RenameColumn(
                name: "AvailableStock",
                table: "ProductStocks",
                newName: "StockStatus_AvailableStock");

            migrationBuilder.AlterColumn<int>(
                name: "StockStatus_ReturnedStock",
                table: "ProductStocks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "StockStatus_QuarantinedStock",
                table: "ProductStocks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "StockStatus_QualityControlStock",
                table: "ProductStocks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "StockStatus_OnHoldStock",
                table: "ProductStocks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "StockStatus_DamagedStock",
                table: "ProductStocks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockStatus_ReturnedStock",
                table: "ProductStocks",
                newName: "ReturnedStock");

            migrationBuilder.RenameColumn(
                name: "StockStatus_QuarantinedStock",
                table: "ProductStocks",
                newName: "QuarantinedStock");

            migrationBuilder.RenameColumn(
                name: "StockStatus_QualityControlStock",
                table: "ProductStocks",
                newName: "QualityControlStock");

            migrationBuilder.RenameColumn(
                name: "StockStatus_OnHoldStock",
                table: "ProductStocks",
                newName: "OnHoldStock");

            migrationBuilder.RenameColumn(
                name: "StockStatus_DamagedStock",
                table: "ProductStocks",
                newName: "DamagedStock");

            migrationBuilder.RenameColumn(
                name: "StockStatus_AvailableStock",
                table: "ProductStocks",
                newName: "AvailableStock");

            migrationBuilder.AlterColumn<int>(
                name: "ReturnedStock",
                table: "ProductStocks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuarantinedStock",
                table: "ProductStocks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QualityControlStock",
                table: "ProductStocks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OnHoldStock",
                table: "ProductStocks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DamagedStock",
                table: "ProductStocks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProductStockStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductStockId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStockStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductStockStatuses_ProductStocks_ProductStockId",
                        column: x => x.ProductStockId,
                        principalTable: "ProductStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStockStatuses_ProductStockId_Status",
                table: "ProductStockStatuses",
                columns: new[] { "ProductStockId", "Status" },
                unique: true);
        }
    }
}
