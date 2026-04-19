using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IcnWorksheet.Migrations
{
    /// <inheritdoc />
    public partial class AddDischargeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DischargeType",
                table: "WardInfectionSurveillance",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DischargeType",
                table: "WardInfectionSurveillance");
        }
    }
}
