using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using LibraryInfrastructure.Controllers;
using LibraryInfrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public class ScientistsController : Controller
    {
        private readonly DbSalaryContext _context;

        public ScientistsController(DbSalaryContext context)
        {
            _context = context;
        }

        // GET: Scientists
        public async Task<IActionResult> Index(int? id, string? name, string? searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var query = _context.Scientists
         .Include(s => s.Department)
             .ThenInclude(d => d.Faculty)
         .Include(s => s.Scientistpositions)
             .ThenInclude(sp => sp.Position)
         .AsQueryable();

            if (id == null)
            {
                ViewBag.DepartmentId = null;
                ViewBag.DepartmentName = "всіх підрозділів";
            }
            else
            {
                ViewBag.DepartmentId = id;
                ViewBag.DepartmentName = name;
                query = query.Where(s => s.Departmentid == id);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchLower = searchString.ToLower();

                query = query.Where(s =>
                    s.Fullname.ToLower().Contains(searchLower) ||
                    s.Department.Name.ToLower().Contains(searchLower) ||
                    s.Department.Faculty.Name.ToLower().Contains(searchLower) ||
                    s.Scientistpositions.Any(sp => sp.Position.Name.ToLower().Contains(searchLower)) 
                );
            }

            return View(await query.ToListAsync());
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
                    ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name");

                    return View(new Scientist { Departmentid = (int)id });
                }
            }

            ViewData["Departmentid"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name");
            return View();
        }

        // POST: Scientists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Fullname,Departmentid,Salary,Createdat,Updatedat")] Scientist scientist, int[] selectedPositionIds)
        {
            ModelState.Remove("Department");

            if (ModelState.IsValid)
            {
                scientist.Createdat = DateTime.Now;
                scientist.Updatedat = DateTime.Now;
                _context.Add(scientist);
                await _context.SaveChangesAsync();

                if (selectedPositionIds != null)
                {
                    foreach (var posId in selectedPositionIds)
                    {
                        _context.Scientistpositions.Add(new Scientistposition
                        {
                            Scientistid = scientist.Id,
                            Positionid = posId,
                            Departmentid = scientist.Departmentid,
                            Employmentrate = 1.0m,
                            Startdate = DateOnly.FromDateTime(DateTime.Now)
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                    var department = await _context.Departments.FindAsync(scientist.Departmentid);
                    return RedirectToAction(nameof(Index), new { id = scientist.Departmentid, name = department?.Name });
                }

                ViewData["Departmentid"] = new SelectList(_context.Departments, "Id", "Name", scientist.Departmentid);
                ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name");
                return View(scientist);
            }

        // GET: Scientists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scientist = await _context.Scientists
                .Include(s => s.Scientistpositions)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scientist == null)
            {
                return NotFound();
            }
            var currentPos = scientist.Scientistpositions.OrderByDescending(p => p.Startdate).FirstOrDefault();

            ViewData["Departmentid"] = new SelectList(_context.Departments, "Id", "Name", scientist.Departmentid);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", currentPos?.Positionid);
            ViewBag.EmploymentRate = currentPos?.Employmentrate;
            return View(scientist);
        }

        // POST: Scientists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Fullname, Departmentid, Salary, Createdat, Updatedat")] Scientist scientist, int[] selectedPositionIds)
            {
            if (id != scientist.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Department");

            if (ModelState.IsValid)
            {
                try
                {
                    scientist.Updatedat = DateTime.Now;
                    _context.Update(scientist);

                    var oldPositions = _context.Scientistpositions.Where(sp => sp.Scientistid == id);
                    _context.Scientistpositions.RemoveRange(oldPositions);

                    if (selectedPositionIds != null)
                    {
                        foreach (var posId in selectedPositionIds)
                        {
                            _context.Scientistpositions.Add(new Scientistposition
                            {
                                Scientistid = id,
                                Positionid = posId,
                                Departmentid = scientist.Departmentid,
                                Employmentrate = 1.0m,
                                Startdate = DateOnly.FromDateTime(DateTime.Now)
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScientistExists(scientist.Id)) return NotFound();
                    else throw;
                }

                var dept = await _context.Departments.FindAsync(scientist.Departmentid);
                return RedirectToAction(nameof(Index), new { id = scientist.Departmentid, name = dept?.Name });
            }
            ViewData["Departmentid"] = new SelectList(_context.Departments, "Id", "Name", scientist.Departmentid);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name");
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
            var scientist = await _context.Scientists
        .Include(s => s.Department)
        .FirstOrDefaultAsync(s => s.Id == id);

            if (scientist != null)
            {
                var deptId = scientist.Departmentid;
                var deptName = scientist.Department?.Name;

                _context.Scientists.Remove(scientist);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { id = deptId, name = deptName });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ScientistExists(int id)
        {
            return _context.Scientists.Any(e => e.Id == id);
        }
    }
}
