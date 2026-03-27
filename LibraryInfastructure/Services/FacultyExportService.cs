using ClosedXML.Excel;
using LibraryInfrastructure.Controllers;
using LibraryInfrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryInfrastructure.Services
{
    public class FacultyExportService : IExportService<Faculty>
    {
        private readonly DbSalaryContext _context;
        public FacultyExportService(DbSalaryContext context) { _context = context; }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            var faculties = await _context.Faculties
                .Include(f => f.Departments)
                    .ThenInclude(d => d.Scientists)
                        .ThenInclude(s => s.Scientistpositions)
                            .ThenInclude(sp => sp.Position)
                .ToListAsync(cancellationToken);

            using var workbook = new XLWorkbook();

            foreach (var faculty in faculties)
            {
                var worksheet = workbook.Worksheets.Add(faculty.Name.Length > 30 ? faculty.Name.Substring(0, 30) : faculty.Name);

                worksheet.Cell(1, 1).Value = "Кафедра";
                worksheet.Cell(1, 2).Value = "ПІБ Науковця";
                worksheet.Cell(1, 3).Value = "Зарплата";
                worksheet.Cell(1, 4).Value = "Посади";

                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#E2EFDA");
                headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int row = 2;
                foreach (var dept in faculty.Departments)
                {
                    foreach (var scientist in dept.Scientists)
                    {
                        worksheet.Cell(row, 1).Value = dept.Name;
                        worksheet.Cell(row, 2).Value = scientist.Fullname;
                        worksheet.Cell(row, 3).Value = scientist.Salary;

                        var posList = scientist.Scientistpositions.Select(sp => sp.Position?.Name).Where(n => n != null);
                        worksheet.Cell(row, 4).Value = string.Join(", ", posList);

                        row++;
                    }
                }
                worksheet.Columns().AdjustToContents();
            }
            workbook.SaveAs(stream);
        }
    }
}
