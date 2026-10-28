
using System.ComponentModel.DataAnnotations;

namespace MedicineInventory.Models;

public class CreateMedicineViewModel : IValidatableObject
{
    [Required(ErrorMessage = "Medicine name is required.")]
    [StringLength(100, MinimumLength = 2)]
    [Display(Name = "Medicine Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required.")]
    [StringLength(50)]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Manufacturer is required.")]
    [StringLength(100)]
    public string Manufacturer { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    [Display(Name = "Minimum Stock Threshold")]
    public int MinimumStockThreshold { get; set; } = 10;

    [Required(ErrorMessage = "Batch number is required.")]
    [StringLength(50)]
    [Display(Name = "Batch Number")]
    public string BatchNumber { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    [Display(Name = "Quantity in Stock")]
    public int Quantity { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Manufacturing Date")]
    public DateTime ManufacturingDate { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    [Display(Name = "Expiry Date")]
    public DateTime ExpiryDate { get; set; } =
        DateTime.Today.AddYears(1);

    [Range(typeof(decimal), "0", "99999999")]
    [Display(Name = "Purchase Price")]
    public decimal PurchasePrice { get; set; }

    [Range(typeof(decimal), "0", "99999999")]
    [Display(Name = "Selling Price")]
    public decimal SellingPrice { get; set; }

    [Required(ErrorMessage = "Supplier name is required.")]
    [StringLength(100)]
    [Display(Name = "Supplier Name")]
    public string SupplierName { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        if (ExpiryDate.Date <= ManufacturingDate.Date)
        {
            yield return new ValidationResult(
                "Expiry date must be later than manufacturing date.",
                new[]
                {
                    nameof(ExpiryDate),
                    nameof(ManufacturingDate)
                });
        }
    }
}
