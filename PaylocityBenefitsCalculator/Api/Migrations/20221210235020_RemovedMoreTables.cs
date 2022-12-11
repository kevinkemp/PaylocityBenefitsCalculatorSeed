using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class RemovedMoreTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deductions");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Paychecks");

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalAnnualDeduction",
                table: "Paychecks",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalDeductionPerDependent",
                table: "Paychecks",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DeductionsPerDependent",
                table: "Paychecks",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyBaseDeduction",
                table: "Paychecks",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IncursAdditionalAnnualCost",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalAnnualDeduction",
                table: "Paychecks");

            migrationBuilder.DropColumn(
                name: "AdditionalDeductionPerDependent",
                table: "Paychecks");

            migrationBuilder.DropColumn(
                name: "DeductionsPerDependent",
                table: "Paychecks");

            migrationBuilder.DropColumn(
                name: "MonthlyBaseDeduction",
                table: "Paychecks");

            migrationBuilder.DropColumn(
                name: "IncursAdditionalAnnualCost",
                table: "Employees");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Paychecks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Deductions",
                columns: table => new
                {
                    DeductionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deductions", x => x.DeductionId);
                    table.ForeignKey(
                        name: "FK_Deductions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deductions_EmployeeId",
                table: "Deductions",
                column: "EmployeeId");
        }
    }
}
