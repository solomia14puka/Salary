using LibraryInfrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryInfrastructure.Controllers;
public class ScientistPositionsController : Controller
{
    private readonly DbSalaryContext _context;

    public ScientistPositionsController(DbSalaryContext context)
    {
        _context = context;
    }

    // 1. Список усіх призначень
    public async Task<IActionResult> Index()
    {
        var positions = _context.Scientistpositions
            .Include(s => s.Scientist)
            .Include(s => s.Position)
            .Include(s => s.Department);
        return View(await positions.ToListAsync());
    }

    // 2. GET: Форма створення
    public IActionResult Create()
    {
        ViewData["Departmentid"] = new SelectList(_context.Departments, "Id", "Name");
        ViewData["Positionid"] = new SelectList(_context.Positions, "Id", "Name");
        ViewData["Scientistid"] = new SelectList(_context.Scientists, "Id", "Fullname");
        return View();
    }

    // 3. POST: Збереження призначення
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Scientistid,Positionid,Departmentid,Employmentrate,Startdate,Enddate")] Scientistposition scientistPosition)
    {
        ModelState.Remove("Department");
        ModelState.Remove("Position");
        ModelState.Remove("Scientist");

        if (ModelState.IsValid)
        {
            _context.Add(scientistPosition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["Departmentid"] = new SelectList(_context.Departments, "Id", "Name", scientistPosition.Departmentid);
        ViewData["Positionid"] = new SelectList(_context.Positions, "Id", "Name", scientistPosition.Positionid);
        ViewData["Scientistid"] = new SelectList(_context.Scientists, "Id", "Fullname", scientistPosition.Scientistid);
        return View(scientistPosition);
    }
}
