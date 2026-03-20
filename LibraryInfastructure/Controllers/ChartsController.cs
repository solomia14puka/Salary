using LibraryInfrastructure.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryInfrastructure.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly DbSalaryContext _context;

        public ChartsController(DbSalaryContext context)
        {
            _context = context;
        }

        // Точка доступу 1: Кількість кафедр на кожному факультеті
        [HttpGet("countDepartments")]
        public async Task<JsonResult> GetDepartmentsPerFacultyAsync()
        {
            var responseItems = await _context.Faculties
                .Select(f => new
                {
                    name = f.Name,
                    count = f.Departments.Count()
                })
                .ToListAsync();

            return new JsonResult(responseItems);
        }

        // Точка доступу 2: Кількість науковців на кожній кафедрі
        [HttpGet("countScientists")]
        public async Task<JsonResult> GetScientistsPerDepartmentAsync()
        {
            var responseItems = await _context.Departments
                .Select(d => new
                {
                    name = d.Name,
                    count = d.Scientists.Count()
                })
                .ToListAsync();

            return new JsonResult(responseItems);
        }
    }
}
