using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryInfrastructure.Models;
using System.Linq;
using System.Threading.Tasks;
using System;
using ClosedXML.Excel;
using System.IO;

namespace LibraryInfrastructure.Controllers
{
    public class FacultyBudgetInfo
    {
        public string FacultyName { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal AverageSalary { get; set; }
    }

    public class AnalyticsController : Controller
    {
        private readonly DbSalaryContext _context;

        public AnalyticsController(DbSalaryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> FacultyBudget()
        {
            var stats = await _context.Scientists
                .Include(s => s.Department)
                    .ThenInclude(d => d.Faculty)
                .Where(s => s.Department != null && s.Department.Faculty != null && s.Salary > 0)
                .GroupBy(s => s.Department.Faculty.Name)
                .Select(g => new FacultyBudgetInfo
                {
                    FacultyName = g.Key,
                    TotalBudget = g.Sum(s => s.Salary ?? 0),
                    AverageSalary = g.Average(s => s.Salary ?? 0)
                })
                .ToListAsync();

            return View(stats);
        }
        public async Task<IActionResult> ExportFacultyBudgetToExcel()
        {
            var stats = await _context.Scientists
                .Include(s => s.Department)
                    .ThenInclude(d => d.Faculty)
                .Where(s => s.Department != null && s.Department.Faculty != null && s.Salary > 0)
                .GroupBy(s => s.Department.Faculty.Name)
                .Select(g => new FacultyBudgetInfo
                {
                    FacultyName = g.Key,
                    TotalBudget = g.Sum(s => s.Salary ?? 0),
                    AverageSalary = g.Average(s => s.Salary ?? 0)
                })
                .ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Бюджет факультетів");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Назва факультету";
                worksheet.Cell(currentRow, 2).Value = "Загальний бюджет (грн)";
                worksheet.Cell(currentRow, 3).Value = "Середня зарплата (грн)";

                worksheet.Range("A1:C1").Style.Font.Bold = true;

                foreach (var item in stats)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.FacultyName;
                    worksheet.Cell(currentRow, 2).Value = item.TotalBudget;
                    worksheet.Cell(currentRow, 3).Value = Math.Round(item.AverageSalary, 2);
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FacultyBudgetReport.xlsx");
                }
            }
        }
    }
}