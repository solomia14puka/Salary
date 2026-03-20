using ClosedXML.Excel;
using LibraryInfrastructure.Controllers;
using LibraryInfrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryInfrastructure.Services
{
    public class FacultyImportService : IImportService<Faculty>
    {
        private readonly DbSalaryContext _context;
        public FacultyImportService(DbSalaryContext context) { _context = context; }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            using var workbook = new XLWorkbook(stream);
            foreach (var worksheet in workbook.Worksheets)
            {
                var faculty = await _context.Faculties
                    .FirstOrDefaultAsync(f => f.Name == worksheet.Name, cancellationToken);

                if (faculty == null)
                {
                    faculty = new Faculty { Name = worksheet.Name };
                    _context.Faculties.Add(faculty);
                }

            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}