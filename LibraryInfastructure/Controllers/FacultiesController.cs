using ClosedXML.Excel;
using LibraryInfrastructure.Controllers;
using LibraryInfrastructure.Models;
using LibraryInfrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryInfrastructure.Controllers
{
    [Authorize]
    public class FacultiesController : Controller
    {
        private readonly DbSalaryContext _context;
        private readonly IDataPortServiceFactory<Faculty> _facultyDataPortServiceFactory;

        public FacultiesController(DbSalaryContext context, IDataPortServiceFactory<Faculty> facultyDataPortServiceFactory)
        {
            _context = context;
            _facultyDataPortServiceFactory = facultyDataPortServiceFactory;
        }

        // GET: Faculties
        public async Task<IActionResult> Index()
        {
            return View(await _context.Faculties.ToListAsync());
        }

        // GET: Faculties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Departments", new { id = faculty.Id, name = faculty.Name });
        }

        // GET: Faculties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faculties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Createdat,Updatedat")] Faculty faculty)
        {
            if (_context.Faculties.Any(f => f.Name == faculty.Name))
            {
                ModelState.AddModelError("Name", "Факультет з такою назвою вже існує!");
            }

            if (ModelState.IsValid)
            {
                _context.Add(faculty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faculty);
        }

        // GET: Faculties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }
            return View(faculty);
        }

        // POST: Faculties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Createdat,Updatedat")] Faculty faculty)
        {
            if (id != faculty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faculty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultyExists(faculty.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(faculty);
        }

        // GET: Faculties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty != null)
            {
                _context.Faculties.Remove(faculty);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacultyExists(int id)
        {
            return _context.Faculties.Any(e => e.Id == id);
        }
        public IActionResult Import() => View();
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (fileExcel == null || fileExcel.Length == 0 || !fileExcel.FileName.EndsWith(".xlsx"))
            {
                TempData["Error"] = "Помилка: файл не обрано або він має неправильний формат (.xlsx).";
                return RedirectToAction(nameof(Index));
            }

            var unknownPositions = new HashSet<string>();
            var errorsList = new List<string>(); 
            int addedCount = 0;
            int updatedCount = 0;

            using (var stream = new MemoryStream())
            {
                await fileExcel.CopyToAsync(stream);
                using (var workbook = new ClosedXML.Excel.XLWorkbook(stream))
                {
                    foreach (var worksheet in workbook.Worksheets)
                    {
                        var rows = worksheet.RangeUsed()?.RowsUsed();
                        if (rows == null) continue;

                        foreach (var row in rows)
                        {
                            string deptName = row.Cell(1).GetValue<string>().Trim();
                            string scName = row.Cell(2).GetValue<string>().Trim();
                            string salaryText = row.Cell(3).GetValue<string>().Trim();
                            string posText = row.Cell(4).GetValue<string>().Trim();

                            if (string.IsNullOrWhiteSpace(scName) || scName == "ПІБ Науковця" || deptName == "Кафедра") continue;

                            var dept = await _context.Departments.FirstOrDefaultAsync(d => d.Name == deptName);
                            if (dept == null)
                            {
                                errorsList.Add($"Аркуш '{worksheet.Name}': Кафедру '{deptName}' не знайдено. Рядок з '{scName}' пропущено.");
                                continue;
                            }

                            if (!decimal.TryParse(salaryText, out decimal parsedSalary) || parsedSalary < 0)
                            {
                                errorsList.Add($"Аркуш '{worksheet.Name}': Некоректна зарплата '{salaryText}' для '{scName}'.");
                                continue;
                            }

                            var scientist = await _context.Scientists.FirstOrDefaultAsync(s => s.Fullname == scName);
                            if (scientist == null)
                            {
                                scientist = new Scientist { Fullname = scName, Createdat = DateTime.Now };
                                _context.Scientists.Add(scientist);
                                addedCount++;
                            }
                            else
                            {
                                updatedCount++;
                            }

                            scientist.Salary = parsedSalary;
                            scientist.Departmentid = dept.Id;
                            scientist.Updatedat = DateTime.Now;

                            await _context.SaveChangesAsync();

                            var oldPos = _context.Scientistpositions.Where(sp => sp.Scientistid == scientist.Id);
                            _context.Scientistpositions.RemoveRange(oldPos);

                            if (!string.IsNullOrWhiteSpace(posText))
                            {
                                var pNames = posText.Split(',')
                                                  .Select(p => p.Trim())
                                                  .Where(p => !string.IsNullOrEmpty(p))
                                                  .Distinct(StringComparer.OrdinalIgnoreCase);

                                foreach (var name in pNames)
                                {
                                    var posEntity = await _context.Positions
                                        .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

                                    if (posEntity != null)
                                    {
                                        _context.Scientistpositions.Add(new Scientistposition
                                        {
                                            Scientistid = scientist.Id,
                                            Positionid = posEntity.Id,
                                            Departmentid = dept.Id,
                                            Employmentrate = 1.0m,
                                            Startdate = DateOnly.FromDateTime(DateTime.Now)
                                        });
                                    }
                                    else
                                    {
                                        unknownPositions.Add(name);
                                    }
                                }
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            string statusMsg = $"Імпорт завершено. Додано: {addedCount}, Оновлено: {updatedCount}.";

            if (errorsList.Any() || unknownPositions.Any())
            {
                string errorDetails = "";
                if (errorsList.Any()) errorDetails += "Пропущено рядків: " + errorsList.Count + ". ";
                if (unknownPositions.Any()) errorDetails += "Невідомі посади: " + string.Join(", ", unknownPositions);

                TempData["Error"] = statusMsg + " " + errorDetails;
            }
            else
            {
                TempData["Success"] = statusMsg;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Export(CancellationToken cancellationToken)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var exportService = _facultyDataPortServiceFactory.GetExportService(contentType);

            using var memoryStream = new MemoryStream();
            await exportService.WriteToAsync(memoryStream, cancellationToken);

            return File(
                    memoryStream.ToArray(),
                    contentType,
                    $"faculty_report_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
                );
        }
    }
}