using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class WebApiModelsWarehouseContextv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Revenue = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalRentDuration = table.Column<int>(nullable: false),
                    MonetaryValue = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    BikeLimit = table.Column<int>(nullable: true),
                    LiftPower = table.Column<double>(nullable: true),
                    Assertion = table.Column<int>(nullable: true),
                    Opening = table.Column<int>(nullable: true),
                    RoofRack_LiftPower = table.Column<double>(nullable: true),
                    IsLockable = table.Column<bool>(nullable: true),
                    Weight = table.Column<double>(nullable: true),
                    AppearenceDescription = table.Column<string>(maxLength: 255, nullable: true),
                    TireDimensions = table.Column<string>(maxLength: 255, nullable: true),
                    ChainThickness = table.Column<double>(nullable: true),
                    Type = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventories");
        }
    }
}
