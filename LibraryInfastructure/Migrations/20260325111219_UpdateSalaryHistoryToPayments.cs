using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalaryInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalaryHistoryToPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "changedate",
                table: "salaryhistory");

            migrationBuilder.DropColumn(
                name: "newsalary",
                table: "salaryhistory");

            migrationBuilder.DropColumn(
                name: "oldsalary",
                table: "salaryhistory");

            migrationBuilder.AlterColumn<int>(
                name: "departmentid",
                table: "scientist",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "scientistid",
                table: "salaryhistory",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "salaryhistory",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "salaryhistory",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "salaryhistory",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "facultyid",
                table: "department",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "salaryhistory");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "salaryhistory");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "salaryhistory");

            migrationBuilder.AlterColumn<int>(
                name: "departmentid",
                table: "scientist",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "scientistid",
                table: "salaryhistory",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "changedate",
                table: "salaryhistory",
                type: "timestamp without time zone",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<decimal>(
                name: "newsalary",
                table: "salaryhistory",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "oldsalary",
                table: "salaryhistory",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "facultyid",
                table: "department",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
