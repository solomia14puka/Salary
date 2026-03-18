using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SalaryInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "faculty",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("faculty_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "position",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    basesalary = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("position_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "department",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    facultyid = table.Column<int>(type: "integer", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("department_pkey", x => x.id);
                    table.ForeignKey(
                        name: "department_facultyid_fkey",
                        column: x => x.facultyid,
                        principalTable: "faculty",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "salaryfund",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    periodstart = table.Column<DateOnly>(type: "date", nullable: false),
                    periodend = table.Column<DateOnly>(type: "date", nullable: false),
                    facultyid = table.Column<int>(type: "integer", nullable: true),
                    departmentid = table.Column<int>(type: "integer", nullable: true),
                    totalamount = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true),
                    calculationdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("salaryfund_pkey", x => x.id);
                    table.ForeignKey(
                        name: "salaryfund_departmentid_fkey",
                        column: x => x.departmentid,
                        principalTable: "department",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "salaryfund_facultyid_fkey",
                        column: x => x.facultyid,
                        principalTable: "faculty",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "scientist",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fullname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    departmentid = table.Column<int>(type: "integer", nullable: true),
                    salary = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("scientist_pkey", x => x.id);
                    table.ForeignKey(
                        name: "scientist_departmentid_fkey",
                        column: x => x.departmentid,
                        principalTable: "department",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "salaryhistory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scientistid = table.Column<int>(type: "integer", nullable: true),
                    oldsalary = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    newsalary = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    changedate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    reason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("salaryhistory_pkey", x => x.id);
                    table.ForeignKey(
                        name: "salaryhistory_scientistid_fkey",
                        column: x => x.scientistid,
                        principalTable: "scientist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scientistposition",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scientistid = table.Column<int>(type: "integer", nullable: true),
                    positionid = table.Column<int>(type: "integer", nullable: true),
                    departmentid = table.Column<int>(type: "integer", nullable: true),
                    employmentrate = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: true, defaultValue: 1.0m),
                    startdate = table.Column<DateOnly>(type: "date", nullable: false),
                    enddate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("scientistposition_pkey", x => x.id);
                    table.ForeignKey(
                        name: "scientistposition_departmentid_fkey",
                        column: x => x.departmentid,
                        principalTable: "department",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "scientistposition_positionid_fkey",
                        column: x => x.positionid,
                        principalTable: "position",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "scientistposition_scientistid_fkey",
                        column: x => x.scientistid,
                        principalTable: "scientist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_department_facultyid",
                table: "department",
                column: "facultyid");

            migrationBuilder.CreateIndex(
                name: "IX_salaryfund_departmentid",
                table: "salaryfund",
                column: "departmentid");

            migrationBuilder.CreateIndex(
                name: "IX_salaryfund_facultyid",
                table: "salaryfund",
                column: "facultyid");

            migrationBuilder.CreateIndex(
                name: "IX_salaryhistory_scientistid",
                table: "salaryhistory",
                column: "scientistid");

            migrationBuilder.CreateIndex(
                name: "IX_scientist_departmentid",
                table: "scientist",
                column: "departmentid");

            migrationBuilder.CreateIndex(
                name: "IX_scientistposition_departmentid",
                table: "scientistposition",
                column: "departmentid");

            migrationBuilder.CreateIndex(
                name: "IX_scientistposition_positionid",
                table: "scientistposition",
                column: "positionid");

            migrationBuilder.CreateIndex(
                name: "IX_scientistposition_scientistid",
                table: "scientistposition",
                column: "scientistid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salaryfund");

            migrationBuilder.DropTable(
                name: "salaryhistory");

            migrationBuilder.DropTable(
                name: "scientistposition");

            migrationBuilder.DropTable(
                name: "position");

            migrationBuilder.DropTable(
                name: "scientist");

            migrationBuilder.DropTable(
                name: "department");

            migrationBuilder.DropTable(
                name: "faculty");
        }
    }
}
