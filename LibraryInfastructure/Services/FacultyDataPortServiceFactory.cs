using LibraryInfrastructure.Controllers;
using LibraryInfrastructure.Models;
using System;

namespace LibraryInfrastructure.Services
{
    public class FacultyDataPortServiceFactory : IDataPortServiceFactory<Faculty>
    {
        private readonly DbSalaryContext _context;

        public FacultyDataPortServiceFactory(DbSalaryContext context)
        {
            _context = context;
        }

        public IImportService<Faculty> GetImportService(string contentType)
        {
            if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new FacultyImportService(_context); // Цей клас ми створимо в наступному кроці!
            }
            throw new NotImplementedException($"Немає сервісу імпорту для типу: {contentType}");
        }

        public IExportService<Faculty> GetExportService(string contentType)
        {
            if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new FacultyExportService(_context); // Цей клас ми теж створимо далі
            }
            throw new NotImplementedException($"Немає сервісу експорту для типу: {contentType}");
        }
    }
}