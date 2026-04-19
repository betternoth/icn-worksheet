using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IcnWorksheet.Migrations
{
    /// <inheritdoc />
    public partial class WardInfectionPatientForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "WardInfectionSurveillance");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "WardInfectionSurveillance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WardInfectionSurveillance_PatientId",
                table: "WardInfectionSurveillance",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_WardInfectionSurveillance_Patients_PatientId",
                table: "WardInfectionSurveillance",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WardInfectionSurveillance_Patients_PatientId",
                table: "WardInfectionSurveillance");

            migrationBuilder.DropIndex(
                name: "IX_WardInfectionSurveillance_PatientId",
                table: "WardInfectionSurveillance");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "WardInfectionSurveillance");

            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "WardInfectionSurveillance",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
