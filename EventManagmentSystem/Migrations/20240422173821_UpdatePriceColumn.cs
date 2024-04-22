using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePriceColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
       name: "Price",
       table: "Events",
       type: "decimal(10,2)",
       nullable: false,
       oldClrType: typeof(double),
       oldType: "float",
       oldPrecision: 10,
       oldScale: 2);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
        name: "Price",
        table: "Events",
        type: "float",
        precision: 10,
        scale: 2,
        nullable: false,
        oldClrType: typeof(decimal),
        oldType: "decimal(10,2)");

        }
    }
}
