using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class Orders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer = table.Column<string>(maxLength: 255, nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    CreationTime = table.Column<TimeSpan>(nullable: false),
                    OrderStartDate = table.Column<DateTime>(nullable: false),
                    RoofRackID = table.Column<int>(nullable: true),
                    BicycleRackID = table.Column<int>(nullable: true),
                    InventoryID = table.Column<int>(nullable: true),
                    WheelChainID = table.Column<int>(nullable: true),
                    OrderEndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Orders_Inventories_BicycleRackID",
                        column: x => x.BicycleRackID,
                        principalTable: "Inventories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Inventories_InventoryID",
                        column: x => x.InventoryID,
                        principalTable: "Inventories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Inventories_RoofRackID",
                        column: x => x.RoofRackID,
                        principalTable: "Inventories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Inventories_WheelChainID",
                        column: x => x.WheelChainID,
                        principalTable: "Inventories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BicycleRackID",
                table: "Orders",
                column: "BicycleRackID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_InventoryID",
                table: "Orders",
                column: "InventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RoofRackID",
                table: "Orders",
                column: "RoofRackID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WheelChainID",
                table: "Orders",
                column: "WheelChainID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
