using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryTransactionsAndInventoryReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WarehouseAddress",
                table: "Warehouses",
                newName: "WarehouseAddress_Line1");

            migrationBuilder.AddColumn<string>(
                name: "WarehouseAddress_City",
                table: "Warehouses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WarehouseAddress_Country",
                table: "Warehouses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WarehouseAddress_Line2",
                table: "Warehouses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseAddress_PostalCode",
                table: "Warehouses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseAddress_State",
                table: "Warehouses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvailableStock",
                table: "ProductStocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DamagedStock",
                table: "ProductStocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OnHoldStock",
                table: "ProductStocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QualityControlStock",
                table: "ProductStocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuarantinedStock",
                table: "ProductStocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReturnedStock",
                table: "ProductStocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InventoryStockReservation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReservationReference = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false, defaultValue: new byte[0]),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryStockReservation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactionHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransactionType = table.Column<int>(type: "integer", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "text", nullable: false),
                    SourceWarehouseId = table.Column<int>(type: "integer", nullable: true),
                    DestinationWarehouseId = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false, defaultValue: new byte[0]),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactionHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransactionHeaders_Warehouses_DestinationWarehouse~",
                        column: x => x.DestinationWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryTransactionHeaders_Warehouses_SourceWarehouseId",
                        column: x => x.SourceWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductStockStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductStockId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "InventoryStockReservationLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReservationId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    ReservedQuantity = table.Column<int>(type: "integer", nullable: false),
                    AllocatedQuantity = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false, defaultValue: new byte[0]),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryStockReservationLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryStockReservationLine_InventoryStockReservation_Res~",
                        column: x => x.ReservationId,
                        principalTable: "InventoryStockReservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryStockReservationLine_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactionLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransactionHeaderId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,4)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false, defaultValue: new byte[0]),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactionLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransactionLines_InventoryTransactionHeaders_Trans~",
                        column: x => x.TransactionHeaderId,
                        principalTable: "InventoryTransactionHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryTransactionLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryStockReservation_ReservationReference",
                table: "InventoryStockReservation",
                column: "ReservationReference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryStockReservationLine_ProductId",
                table: "InventoryStockReservationLine",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryStockReservationLine_ReservationId_ProductId",
                table: "InventoryStockReservationLine",
                columns: new[] { "ReservationId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactionHeaders_DestinationWarehouseId",
                table: "InventoryTransactionHeaders",
                column: "DestinationWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactionHeaders_ReferenceNumber",
                table: "InventoryTransactionHeaders",
                column: "ReferenceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactionHeaders_SourceWarehouseId",
                table: "InventoryTransactionHeaders",
                column: "SourceWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactionLines_ProductId",
                table: "InventoryTransactionLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactionLines_TransactionHeaderId",
                table: "InventoryTransactionLines",
                column: "TransactionHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStockStatuses_ProductStockId_Status",
                table: "ProductStockStatuses",
                columns: new[] { "ProductStockId", "Status" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryStockReservationLine");

            migrationBuilder.DropTable(
                name: "InventoryTransactionLines");

            migrationBuilder.DropTable(
                name: "ProductStockStatuses");

            migrationBuilder.DropTable(
                name: "InventoryStockReservation");

            migrationBuilder.DropTable(
                name: "InventoryTransactionHeaders");

            migrationBuilder.DropColumn(
                name: "WarehouseAddress_City",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "WarehouseAddress_Country",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "WarehouseAddress_Line2",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "WarehouseAddress_PostalCode",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "WarehouseAddress_State",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "AvailableStock",
                table: "ProductStocks");

            migrationBuilder.DropColumn(
                name: "DamagedStock",
                table: "ProductStocks");

            migrationBuilder.DropColumn(
                name: "OnHoldStock",
                table: "ProductStocks");

            migrationBuilder.DropColumn(
                name: "QualityControlStock",
                table: "ProductStocks");

            migrationBuilder.DropColumn(
                name: "QuarantinedStock",
                table: "ProductStocks");

            migrationBuilder.DropColumn(
                name: "ReturnedStock",
                table: "ProductStocks");

            migrationBuilder.RenameColumn(
                name: "WarehouseAddress_Line1",
                table: "Warehouses",
                newName: "WarehouseAddress");
        }
    }
}
