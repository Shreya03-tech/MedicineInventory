
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineInventory.Models;

public class Medicine
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Medicine name is required.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Medicine name must be between 2 and 100 characters.")]
    [Display(Name = "Medicine Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required.")]
    [StringLength(50)]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Manufacturer is required.")]
    [StringLength(100)]
    public string Manufacturer { get; set; } = string.Empty;

    [Range(0, int.MaxValue,
        ErrorMessage = "Minimum stock threshold cannot be negative.")]
    [Display(Name = "Minimum Stock Threshold")]
    public int MinimumStockThreshold { get; set; } = 10;

    // One medicine can have multiple batches.
    public ICollection<MedicineBatch> Batches { get; set; }
        = new List<MedicineBatch>();

    // Calculated from all associated batches.
    [NotMapped]
    public int TotalQuantity => Batches.Sum(b => b.Quantity);

    [NotMapped]
    public int BatchCount => Batches.Count;

    [NotMapped]
    public DateTime? NearestExpiryDate =>
        Batches.Where(b => b.Quantity > 0)
            .Select(b => (DateTime?)b.ExpiryDate)
            .Min();

    [NotMapped]
    public bool IsLowStock =>
        TotalQuantity <= MinimumStockThreshold;
}
