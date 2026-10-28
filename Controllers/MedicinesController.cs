
using MedicineInventory.Data;
using MedicineInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicineInventory.Controllers;

public class MedicinesController : Controller
{
    private readonly ApplicationDbContext _context;

    public MedicinesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Medicines
    public async Task<IActionResult> Index(
        string? searchString,
        string? status,
        string? sortOrder)
    {
        var medicines = await _context.Medicines
            .AsNoTracking()
            .Include(m => m.Batches)
            .ToListAsync();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            var term = searchString.Trim();

            medicines = medicines.Where(m =>
                m.Name.Contains(
                    term, StringComparison.OrdinalIgnoreCase) ||
                m.Manufacturer.Contains(
                    term, StringComparison.OrdinalIgnoreCase) ||
                m.Batches.Any(b => b.BatchNumber.Contains(
                    term, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        var today = DateTime.Today;
        var thirtyDays = today.AddDays(30);
        var ninetyDays = today.AddDays(90);

        medicines = status?.ToLowerInvariant() switch
        {
            "expired" => medicines.Where(m =>
                m.Batches.Any(b => b.ExpiryDate.Date < today))
                .ToList(),

            "expiring-soon" => medicines.Where(m =>
                m.Batches.Any(b =>
                    b.ExpiryDate.Date >= today &&
                    b.ExpiryDate.Date <= thirtyDays))
                .ToList(),

            "within-90-days" => medicines.Where(m =>
                m.Batches.Any(b =>
                    b.ExpiryDate.Date > thirtyDays &&
                    b.ExpiryDate.Date <= ninetyDays))
                .ToList(),

            "safe" => medicines.Where(m =>
                m.Batches.Any(b => b.ExpiryDate.Date > ninetyDays))
                .ToList(),

            "low-stock" => medicines.Where(m =>
                m.TotalQuantity <= m.MinimumStockThreshold)
                .ToList(),

            _ => medicines
        };

        medicines = sortOrder switch
        {
            "name_desc" =>
                medicines.OrderByDescending(m => m.Name).ToList(),

            "expiry_asc" =>
                medicines.OrderBy(m => m.NearestExpiryDate).ToList(),

            "expiry_desc" =>
                medicines.OrderByDescending(m => m.NearestExpiryDate).ToList(),

            "quantity_asc" =>
                medicines.OrderBy(m => m.TotalQuantity).ToList(),

            "quantity_desc" =>
                medicines.OrderByDescending(m => m.TotalQuantity).ToList(),

            _ => medicines.OrderBy(m => m.Name).ToList()
        };

        ViewBag.SearchString = searchString;
        ViewBag.Status = status;
        ViewBag.SortOrder = sortOrder;

        return View(medicines);
    }

    // GET: Medicines/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var medicine = await _context.Medicines
            .AsNoTracking()
            .Include(m => m.Batches)
            .FirstOrDefaultAsync(m => m.Id == id.Value);

        if (medicine == null)
            return NotFound();

        return View(medicine);
    }

    // GET: Medicines/Create
    [HttpGet]
    public IActionResult Create()
    {
        var model = new CreateMedicineViewModel
        {
            MinimumStockThreshold = 10,
            ManufacturingDate = DateTime.Today,
            ExpiryDate = DateTime.Today.AddYears(1)
        };

        return View(model);
    }

    // POST: Medicines/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateMedicineViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Confirm that the batch dates are valid before saving.
        if (model.ExpiryDate.Date <= model.ManufacturingDate.Date)
        {
            ModelState.AddModelError(
                nameof(model.ExpiryDate),
                "Expiry date must be later than manufacturing date.");

            return View(model);
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var medicine = new Medicine
            {
                Name = model.Name.Trim(),
                Category = model.Category.Trim(),
                Manufacturer = model.Manufacturer.Trim(),
                MinimumStockThreshold = model.MinimumStockThreshold
            };

            _context.Medicines.Add(medicine);

            // Save first to generate the medicine ID.
            await _context.SaveChangesAsync();

            var batch = new MedicineBatch
            {
                MedicineId = medicine.Id,
                BatchNumber = model.BatchNumber.Trim(),
                Quantity = model.Quantity,
                ManufacturingDate = model.ManufacturingDate.Date,
                ExpiryDate = model.ExpiryDate.Date,
                PurchasePrice = model.PurchasePrice,
                SellingPrice = model.SellingPrice,
                SupplierName = model.SupplierName.Trim()
            };

            _context.MedicineBatches.Add(batch);

            await _context.SaveChangesAsync();

            // Commit both records together.
            await transaction.CommitAsync();

            TempData["SuccessMessage"] =
                "Medicine and its first batch were added successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // GET: Medicines/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var medicine = await _context.Medicines
            .FindAsync(id.Value);

        if (medicine == null)
            return NotFound();

        return View(medicine);
    }

    // POST: Medicines/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        Medicine medicine)
    {
        if (id != medicine.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(medicine);

        var existing = await _context.Medicines
            .FindAsync(id);

        if (existing == null)
            return NotFound();

        existing.Name = medicine.Name.Trim();
        existing.Category = medicine.Category.Trim();
        existing.Manufacturer = medicine.Manufacturer.Trim();
        existing.MinimumStockThreshold =
            medicine.MinimumStockThreshold;

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            "Medicine updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    // GET: Medicines/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var medicine = await _context.Medicines
            .AsNoTracking()
            .Include(m => m.Batches)
            .FirstOrDefaultAsync(m => m.Id == id.Value);

        if (medicine == null)
            return NotFound();

        return View(medicine);
    }

    // POST: Medicines/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var medicine = await _context.Medicines
            .Include(m => m.Batches)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (medicine != null)
        {
            // Delete dependent batches first because the
            // relationship uses DeleteBehavior.Restrict.
            _context.MedicineBatches.RemoveRange(medicine.Batches);
            _context.Medicines.Remove(medicine);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Medicine and its batches deleted successfully.";
        }

        return RedirectToAction(nameof(Index));
    }
}
