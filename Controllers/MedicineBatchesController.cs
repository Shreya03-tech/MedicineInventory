using MedicineInventory.Data;
using MedicineInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MedicineInventory.Controllers;

public class MedicineBatchesController : Controller
{
    private readonly ApplicationDbContext _context;

    public MedicineBatchesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? medicineId, string? status)
    {
        var query = _context.MedicineBatches
            .AsNoTracking()
            .Include(b => b.Medicine)
            .AsQueryable();

        if (medicineId.HasValue)
            query = query.Where(b => b.MedicineId == medicineId.Value);

        var batches = await query.OrderBy(b => b.ExpiryDate).ToListAsync();

        batches = status?.ToLowerInvariant() switch
        {
            "expired" => batches.Where(b => b.ExpiryDate.Date < DateTime.Today).ToList(),
            "expiring-soon" => batches.Where(b => b.ExpiryDate.Date >= DateTime.Today &&
                                                  b.ExpiryDate.Date <= DateTime.Today.AddDays(30)).ToList(),
            "low-stock" => batches.Where(b => b.Quantity <= 10).ToList(),
            _ => batches
        };

        ViewBag.MedicineId = medicineId;
        ViewBag.Status = status;
        ViewBag.MedicineName = medicineId.HasValue
            ? await _context.Medicines.Where(m => m.Id == medicineId.Value)
                .Select(m => m.Name).FirstOrDefaultAsync()
            : null;

        return View(batches);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var batch = await _context.MedicineBatches
            .AsNoTracking()
            .Include(b => b.Medicine)
            .FirstOrDefaultAsync(b => b.Id == id);

        return batch == null ? NotFound() : View(batch);
    }

    public async Task<IActionResult> Create(int? medicineId)
    {
        await PopulateMedicinesAsync(medicineId);
        return View(new MedicineBatch
        {
            MedicineId = medicineId ?? 0,
            ManufacturingDate = DateTime.Today,
            ExpiryDate = DateTime.Today.AddYears(1)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MedicineBatch batch)
    {
        ModelState.Remove(nameof(MedicineBatch.Medicine));
        if (!await _context.Medicines.AnyAsync(m => m.Id == batch.MedicineId))
            ModelState.AddModelError(nameof(batch.MedicineId), "Select a valid medicine.");

        if (!ModelState.IsValid)
        {
            await PopulateMedicinesAsync(batch.MedicineId);
            return View(batch);
        }

        _context.MedicineBatches.Add(batch);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Batch added successfully.";
        return RedirectToAction(nameof(Index), new { medicineId = batch.MedicineId });
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var batch = await _context.MedicineBatches.FindAsync(id);
        if (batch == null) return NotFound();

        await PopulateMedicinesAsync(batch.MedicineId);
        return View(batch);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MedicineBatch input)
    {
        if (id != input.Id) return NotFound();

        ModelState.Remove(nameof(MedicineBatch.Medicine));
        if (!await _context.Medicines.AnyAsync(m => m.Id == input.MedicineId))
            ModelState.AddModelError(nameof(input.MedicineId), "Select a valid medicine.");

        if (!ModelState.IsValid)
        {
            await PopulateMedicinesAsync(input.MedicineId);
            return View(input);
        }

        var batch = await _context.MedicineBatches.FindAsync(id);
        if (batch == null) return NotFound();

        batch.MedicineId = input.MedicineId;
        batch.BatchNumber = input.BatchNumber;
        batch.Quantity = input.Quantity;
        batch.ManufacturingDate = input.ManufacturingDate;
        batch.ExpiryDate = input.ExpiryDate;
        batch.PurchasePrice = input.PurchasePrice;
        batch.SellingPrice = input.SellingPrice;
        batch.SupplierName = input.SupplierName;

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Batch updated successfully.";
        return RedirectToAction(nameof(Index), new { medicineId = batch.MedicineId });
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var batch = await _context.MedicineBatches
            .AsNoTracking()
            .Include(b => b.Medicine)
            .FirstOrDefaultAsync(b => b.Id == id);

        return batch == null ? NotFound() : View(batch);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var batch = await _context.MedicineBatches.FindAsync(id);
        if (batch != null)
        {
            var medicineId = batch.MedicineId;
            _context.MedicineBatches.Remove(batch);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Batch deleted successfully.";
            return RedirectToAction(nameof(Index), new { medicineId });
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateMedicinesAsync(int? selectedId)
    {
        ViewBag.MedicineId = new SelectList(
            await _context.Medicines.AsNoTracking().OrderBy(m => m.Name).ToListAsync(),
            nameof(Medicine.Id),
            nameof(Medicine.Name),
            selectedId);
    }
}
