using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IcnWorksheet.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHospitalNumberFromWardInfectionSurveillance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HospitalNumber",
                table: "WardInfectionSurveillance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HospitalNumber",
                table: "WardInfectionSurveillance",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
