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
    public class DepartmentsController : Controller
    {
        private readonly DbSalaryContext _context;

        public DepartmentsController(DbSalaryContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null)
            {
                ViewBag.FacultyName = "всіх факультетів";
                var allDepartments = _context.Departments.Include(d => d.Faculty);
                return View(await allDepartments.ToListAsync());
            }

            // Якщо ID передано — фільтруємо за конкретним факультетом
            ViewBag.FacultyId = id;
            ViewBag.FacultyName = name;
            var filteredDepartments = _context.Departments
                .Where(d => d.Facultyid == id)
                .Include(d => d.Faculty);

            return View(await filteredDepartments.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Faculty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Scientists", new { id = department.Id, name = department.Name });
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            ViewData["Facultyid"] = new SelectList(_context.Faculties, "Id", "Name");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Facultyid,Createdat,Updatedat")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Facultyid"] = new SelectList(_context.Faculties, "Id", "Name", department.Facultyid);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["Facultyid"] = new SelectList(_context.Faculties, "Id", "Name", department.Facultyid);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Facultyid,Createdat,Updatedat")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
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
            ViewData["Facultyid"] = new SelectList(_context.Faculties, "Id", "Name", department.Facultyid);
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Faculty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
