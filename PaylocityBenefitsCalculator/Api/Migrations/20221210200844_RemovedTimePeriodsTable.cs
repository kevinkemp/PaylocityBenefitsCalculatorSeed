using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class RemovedTimePeriodsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paychecks_TimePeriods_PeriodId",
                table: "Paychecks");

            migrationBuilder.DropTable(
                name: "TimePeriods");

            migrationBuilder.DropIndex(
                name: "IX_Paychecks_PeriodId",
                table: "Paychecks");

            migrationBuilder.DropColumn(
                name: "PeriodId",
                table: "Paychecks");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Paychecks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PayDate",
                table: "Paychecks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Paychecks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Paychecks");

            migrationBuilder.DropColumn(
                name: "PayDate",
                table: "Paychecks");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Paychecks");

            migrationBuilder.AddColumn<int>(
                name: "PeriodId",
                table: "Paychecks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TimePeriods",
                columns: table => new
                {
                    PeriodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimePeriods", x => x.PeriodId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Paychecks_PeriodId",
                table: "Paychecks",
                column: "PeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Paychecks_TimePeriods_PeriodId",
                table: "Paychecks",
                column: "PeriodId",
                principalTable: "TimePeriods",
                principalColumn: "PeriodId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
