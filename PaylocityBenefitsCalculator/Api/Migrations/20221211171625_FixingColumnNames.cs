using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class FixingColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalDeductions",
                table: "Paychecks",
                newName: "TotalDeductionsPerPaycheck");

            migrationBuilder.RenameColumn(
                name: "NetPay",
                table: "Paychecks",
                newName: "NetPayPerPaycheck");

            migrationBuilder.RenameColumn(
                name: "MonthlyBaseDeduction",
                table: "Paychecks",
                newName: "MonthlyBaseDeductionPerPaycheck");

            migrationBuilder.RenameColumn(
                name: "GrossPay",
                table: "Paychecks",
                newName: "GrossPayPerPaycheck");

            migrationBuilder.RenameColumn(
                name: "DeductionsPerDependent",
                table: "Paychecks",
                newName: "DeductionsPerDependentPerPaycheck");

            migrationBuilder.RenameColumn(
                name: "AdditionalDeductionPerDependent",
                table: "Paychecks",
                newName: "AdditionalYearlyDeductionPerPaycheck");

            migrationBuilder.RenameColumn(
                name: "AdditionalAnnualDeduction",
                table: "Paychecks",
                newName: "AdditionalDeductionPerDependentPerPaycheck");

            migrationBuilder.RenameColumn(
                name: "IncursAdditionalAnnualCost",
                table: "Employees",
                newName: "IncursAdditionalYearlyCost");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalDeductionsPerPaycheck",
                table: "Paychecks",
                newName: "TotalDeductions");

            migrationBuilder.RenameColumn(
                name: "NetPayPerPaycheck",
                table: "Paychecks",
                newName: "NetPay");

            migrationBuilder.RenameColumn(
                name: "MonthlyBaseDeductionPerPaycheck",
                table: "Paychecks",
                newName: "MonthlyBaseDeduction");

            migrationBuilder.RenameColumn(
                name: "GrossPayPerPaycheck",
                table: "Paychecks",
                newName: "GrossPay");

            migrationBuilder.RenameColumn(
                name: "DeductionsPerDependentPerPaycheck",
                table: "Paychecks",
                newName: "DeductionsPerDependent");

            migrationBuilder.RenameColumn(
                name: "AdditionalYearlyDeductionPerPaycheck",
                table: "Paychecks",
                newName: "AdditionalDeductionPerDependent");

            migrationBuilder.RenameColumn(
                name: "AdditionalDeductionPerDependentPerPaycheck",
                table: "Paychecks",
                newName: "AdditionalAnnualDeduction");

            migrationBuilder.RenameColumn(
                name: "IncursAdditionalYearlyCost",
                table: "Employees",
                newName: "IncursAdditionalAnnualCost");
        }
    }
}
