
using System.Diagnostics;
using MedicineInventory.Data;
using MedicineInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicineInventory.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(
        ILogger<HomeController> logger,
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;
        var thirtyDays = today.AddDays(30);
        var ninetyDays = today.AddDays(90);

        var medicines = await _context.Medicines
            .AsNoTracking()
            .Include(m => m.Batches)
            .ToListAsync();

        var dashboard = new DashboardViewModel
        {
            TotalMedicines = medicines.Count,

            ExpiredMedicines = medicines.Count(m =>
                m.Batches.Any(b => b.ExpiryDate.Date < today)),

            ExpiringSoonMedicines = medicines.Count(m =>
                m.Batches.Any(b =>
                    b.ExpiryDate.Date >= today &&
                    b.ExpiryDate.Date <= thirtyDays)),

            ExpiringWithin90DaysMedicines = medicines.Count(m =>
                m.Batches.Any(b =>
                    b.ExpiryDate.Date > thirtyDays &&
                    b.ExpiryDate.Date <= ninetyDays)),

            SafeMedicines = medicines.Count(m =>
                m.Batches.Any(b => b.ExpiryDate.Date > ninetyDays)),

            LowStockMedicines = medicines.Count(m =>
                m.TotalQuantity <= m.MinimumStockThreshold),

            NormalStockMedicines = medicines.Count(m =>
                m.TotalQuantity > m.MinimumStockThreshold),

            RecentMedicines = medicines
                .OrderByDescending(m => m.Id)
                .Take(5)
                .ToList()
        };

        return View(dashboard);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id
                ?? HttpContext.TraceIdentifier
        });
    }
}
