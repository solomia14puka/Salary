using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryInfrastructure.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace LibraryInfrastructure.Controllers
{
    public class SalaryFundController : Controller
    {
        private readonly DbSalaryContext _context;

        public SalaryFundController(DbSalaryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentDate = DateTime.Now;
            var culture = new CultureInfo("uk-UA");

            var currentMonthTotal = await _context.Salaryhistories
                .Where(s => s.PaymentDate.HasValue
                         && s.PaymentDate.Value.Month == currentDate.Month
                         && s.PaymentDate.Value.Year == currentDate.Year)
                .SumAsync(s => s.Amount) ?? 0;
            decimal reservePercentage = 0.05m;
            var plannedFund = currentMonthTotal + (currentMonthTotal * reservePercentage);
            ViewBag.CurrentMonthName = currentDate.ToString("MMMM yyyy", culture);
            ViewBag.NextMonthName = currentDate.AddMonths(1).ToString("MMMM yyyy", culture);
            ViewBag.CurrentTotal = currentMonthTotal;
            ViewBag.PlannedFund = plannedFund;

            return View();
        }
    }
}