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
    public class ScientistsController : Controller
    {
        private readonly DbSalaryContext _context;

        public ScientistsController(DbSalaryContext context)
        {
            _context = context;
        }

        // GET: Scientists
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null)
            {
                ViewBag.DepartmentName = "всіх підрозділів";
                var allScientists = _context.Scientists.Include(s => s.Department);
                return View(await allScientists.ToListAsync());
            }

            // Якщо ID передано — фільтруємо за кафедрою
            ViewBag.DepartmentId = id;
            ViewBag.DepartmentName = name;
            var filteredScientists = _context.Scientists
                .Where(s => s.Departmentid == id)
                .Include(s => s.Department);

            return View(await filteredScientists.ToListAsync());
        }

        // GET: Scientists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scientist = await _context.Scientists
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scientist == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Salaryhistories", new { id = scientist.Id, name = scientist.Fullname });
        }

        // GET: Scientists/Create
        public IActionResult Create(int? id)
        {
            if (id != null)
            {
                var department = _context.Departments.Find(id);
                if (department != null)
                {
                    ViewBag.DepartmentName = department.Name;

                    return View(new Scientist { Departmentid = (int)id });
                }
            }

            ViewData["Departmentid"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Scientists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Fullname,Departmentid,Salary,Createdat,Updatedat")] Scientist scientist)
        {
            ModelState.Remove("Department");

            if (ModelState.IsValid)
            {
                _context.Add(scientist);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { id = scientist.Departmentid });
            }

            ViewData["Departmentid"] = new SelectList(_context.Departments, "Id", "Name", scientist.Departmentid);
            return View(scientist);
        }

        // GET: Scientists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scientist = await _context.Scientists.FindAsync(id);
            if (scientist == null)
            {
                return NotFound();
            }
            ViewData["Departmentid"] = new SelectList(_context.Departments, "Id", "Name", scientist.Departmentid);
            return View(scientist);
        }

        // POST: Scientists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fullname,Departmentid,Salary,Createdat,Updatedat")] Scientist scientist)
        {
            if (id != scientist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scientist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScientistExists(scientist.Id))
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
            ViewData["Departmentid"] = new SelectList(_context.Departments, "Id", "Name", scientist.Departmentid);
            return View(scientist);
        }

        // GET: Scientists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scientist = await _context.Scientists
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scientist == null)
            {
                return NotFound();
            }

            return View(scientist);
        }

        // POST: Scientists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scientist = await _context.Scientists.FindAsync(id);
            if (scientist != null)
            {
                _context.Scientists.Remove(scientist);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScientistExists(int id)
        {
            return _context.Scientists.Any(e => e.Id == id);
        }
    }
}
