using DocumentFormat.OpenXml.InkML;
using LibraryInfrastructure.Controllers;
using LibraryInfrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryInfrastructure.Controllers
{
    public class SalaryhistoriesController : Controller
    {
        private readonly DbSalaryContext _context;

        public SalaryhistoriesController(DbSalaryContext context)
        {
            _context = context;
        }

        // GET: Salaryhistories
        public async Task<IActionResult> Index()
        {
            var dbSalaryContext = _context.Salaryhistories.Include(s => s.Scientist);
            return View(await dbSalaryContext.ToListAsync());
        }

        // GET: Salaryhistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryhistory = await _context.Salaryhistories
                .Include(s => s.Scientist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salaryhistory == null)
            {
                return NotFound();
            }

            return View(salaryhistory);
        }

        // GET: Salaryhistories/Create
        public IActionResult Create()
        {
            ViewData["Scientistid"] = new SelectList(_context.Scientists, "Id", "Fullname");
            return View();
        }

        // POST: Salaryhistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Scientistid,Amount,PaymentType,PaymentDate,Reason")] Salaryhistory salaryhistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salaryhistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Scientistid"] = new SelectList(_context.Scientists, "Id", "Fullname", salaryhistory.Scientistid);
            return View(salaryhistory);
        }

        // GET: Salaryhistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryhistory = await _context.Salaryhistories.FindAsync(id);
            if (salaryhistory == null)
            {
                return NotFound();
            }
            ViewData["Scientistid"] = new SelectList(_context.Scientists, "Id", "Fullname", salaryhistory.Scientistid);
            return View(salaryhistory);
        }

        // POST: Salaryhistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Scientistid,Amount,PaymentType,PaymentDate,Reason")] Salaryhistory salaryhistory)
        {
            if (id != salaryhistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salaryhistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryhistoryExists(salaryhistory.Id))
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
            ViewData["Scientistid"] = new SelectList(_context.Scientists, "Id", "Fullname", salaryhistory.Scientistid);
            return View(salaryhistory);
        }

        // GET: Salaryhistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryhistory = await _context.Salaryhistories
                .Include(s => s.Scientist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salaryhistory == null)
            {
                return NotFound();
            }

            return View(salaryhistory);
        }

        // POST: Salaryhistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salaryhistory = await _context.Salaryhistories.FindAsync(id);
            if (salaryhistory != null)
            {
                _context.Salaryhistories.Remove(salaryhistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryhistoryExists(int id)
        {
            return _context.Salaryhistories.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetScientistSalary(int id)
        {
            var scientist = await _context.Scientists.FindAsync(id);

            if (scientist != null)
            {
                return Json(new { salary = scientist.Salary });
            }
            return Json(new { salary = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AutoPayBaseSalaries(DateTime paymentDate)
        {
            var scientists = await _context.Scientists.ToListAsync();
            var newPayments = new List<Salaryhistory>();

            foreach (var scientist in scientists)
            {
                if (scientist.Salary != null && scientist.Salary > 0)
                {
                    newPayments.Add(new Salaryhistory
                    {
                        Scientistid = scientist.Id,
                        Amount = scientist.Salary,
                        PaymentDate = paymentDate,
                        PaymentType = "Основна ставка",
                        Reason = "Автоматичне нарахування основного окладу"
                    });
                }
            }

            if (newPayments.Any())
            {
                _context.Salaryhistories.AddRange(newPayments);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Dynamics()
        {
            var rawData = await _context.Salaryhistories
                .Where(h => h.PaymentDate != null) 
                .GroupBy(h => new { h.PaymentDate.Value.Year, h.PaymentDate.Value.Month })
                .Select(g => new {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalAmount = g.Sum(h => h.Amount)
                })
                .ToListAsync();

            var dynamics = rawData
                .Select(d => new {
                    Period = $"{d.Month:D2}/{d.Year}",
                    TotalAmount = d.TotalAmount,
                    Date = new DateTime(d.Year, d.Month, 1) 
                })
                .OrderBy(d => d.Date)
                .ToList();

            return View(dynamics);
        }
        public async Task<IActionResult> ExportDynamicsReport()
        {
            // Робимо безпечний запит з .Where та .Value
            var data = await _context.Salaryhistories
                .Where(h => h.PaymentDate != null)
                .GroupBy(h => new { h.PaymentDate.Value.Year, h.PaymentDate.Value.Month })
                .Select(g => new {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Sum(h => h.Amount)
                })
                .OrderByDescending(g => g.Year).ThenByDescending(g => g.Month)
                .ToListAsync();

            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Звіт по динаміці");
                worksheet.Cell(1, 1).Value = "Рік";
                worksheet.Cell(1, 2).Value = "Місяць";
                worksheet.Cell(1, 3).Value = "Фонд зарплати (грн)";

                var header = worksheet.Row(1);
                header.Style.Font.Bold = true;
                header.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;

                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = data[i].Year;
                    worksheet.Cell(i + 2, 2).Value = data[i].Month;
                    worksheet.Cell(i + 2, 3).Value = data[i].Total;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"SalaryDynamics_{DateTime.Now:yyyyMMdd}.xlsx");
                }
            }
        }
    }
}

