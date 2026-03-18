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
    public class ScientistpositionsController : Controller
    {
        private readonly DbSalaryContext _context;

        public ScientistpositionsController(DbSalaryContext context)
        {
            _context = context;
        }

        // GET: Scientistpositions
        public async Task<IActionResult> Index()
        {
            var dbSalaryContext = _context.Scientistpositions.Include(s => s.Department).Include(s => s.Position).Include(s => s.Scientist);
            return View(await dbSalaryContext.ToListAsync());
        }

        // GET: Scientistpositions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scientistposition = await _context.Scientistpositions
                .Include(s => s.Department)
                .Include(s => s.Position)
                .Include(s => s.Scientist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scientistposition == null)
            {
                return NotFound();
            }

            return View(scientistposition);
        }

        // GET: Scientistpositions/Create
        public IActionResult Create()
        {
            ViewData["Departmentid"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["Positionid"] = new SelectList(_context.Positions, "Id", "Id");
            ViewData["Scientistid"] = new SelectList(_context.Scientists, "Id", "Fullname");
            return View();
        }

        // POST: Scientistpositions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Scientistid,Positionid,Departmentid,Employmentrate,Startdate,Enddate")] Scientistposition scientistposition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scientistposition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Departmentid"] = new SelectList(_context.Departments, "Id", "Name", scientistposition.Departmentid);
            ViewData["Positionid"] = new SelectList(_context.Positions, "Id", "Id", scientistposition.Positionid);
            ViewData["Scientistid"] = new SelectList(_context.Scientists, "Id", "Fullname", scientistposition.Scientistid);
            return View(scientistposition);
        }

        // GET: Scientistpositions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scientistposition = await _context.Scientistpositions.FindAsync(id);
            if (scientistposition == null)
            {
                return NotFound();
            }
            ViewData["Departmentid"] = new SelectList(_context.Departments, "Id", "Name", scientistposition.Departmentid);
            ViewData["Positionid"] = new SelectList(_context.Positions, "Id", "Id", scientistposition.Positionid);
            ViewData["Scientistid"] = new SelectList(_context.Scientists, "Id", "Fullname", scientistposition.Scientistid);
            return View(scientistposition);
        }

        // POST: Scientistpositions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Scientistid,Positionid,Departmentid,Employmentrate,Startdate,Enddate")] Scientistposition scientistposition)
        {
            if (id != scientistposition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scientistposition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScientistpositionExists(scientistposition.Id))
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
            ViewData["Departmentid"] = new SelectList(_context.Departments, "Id", "Name", scientistposition.Departmentid);
            ViewData["Positionid"] = new SelectList(_context.Positions, "Id", "Id", scientistposition.Positionid);
            ViewData["Scientistid"] = new SelectList(_context.Scientists, "Id", "Fullname", scientistposition.Scientistid);
            return View(scientistposition);
        }

        // GET: Scientistpositions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scientistposition = await _context.Scientistpositions
                .Include(s => s.Department)
                .Include(s => s.Position)
                .Include(s => s.Scientist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scientistposition == null)
            {
                return NotFound();
            }

            return View(scientistposition);
        }

        // POST: Scientistpositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scientistposition = await _context.Scientistpositions.FindAsync(id);
            if (scientistposition != null)
            {
                _context.Scientistpositions.Remove(scientistposition);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScientistpositionExists(int id)
        {
            return _context.Scientistpositions.Any(e => e.Id == id);
        }
    }
}
