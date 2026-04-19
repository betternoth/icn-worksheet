using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IcnWorksheet.Migrations
{
    /// <inheritdoc />
    public partial class AddTransferWardsToWardInfectionSurveillance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransferFromWard",
                table: "WardInfectionSurveillance",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransferToWard",
                table: "WardInfectionSurveillance",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferFromWard",
                table: "WardInfectionSurveillance");

            migrationBuilder.DropColumn(
                name: "TransferToWard",
                table: "WardInfectionSurveillance");
        }
    }
}
