using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryInfrastructure.Controllers;
using LibraryInfrastructure.Models;

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
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Index", "Scientists");
            ViewBag.ScientistId = id;
            ViewBag.ScientistName = name;
            var dbSalaryContext = _context.Salaryhistories
        .Where(s => s.Scientistid == id)
        .Include(s => s.Scientist);
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
        public IActionResult Create(int id)
        {
            ViewData["Scientistid"] = new SelectList(_context.Scientists, "Id", "Fullname", id);
            var scientist = _context.Scientists.Find(id);
            if (scientist == null) return NotFound();
            ViewBag.ScientistName = scientist.Fullname;
            var salaryRecord = new Salaryhistory { Scientistid = id };
            return View(salaryRecord);
        }

        // POST: Salaryhistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Scientistid,Oldsalary,Newsalary,Changedate,Reason")] Salaryhistory salaryhistory)
        {
            ModelState.Remove("Scientist");
            if (ModelState.IsValid)
            {
                _context.Add(salaryhistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {id= salaryhistory.Scientistid});
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Scientistid,Oldsalary,Newsalary,Changedate,Reason")] Salaryhistory salaryhistory)
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
    }
}
