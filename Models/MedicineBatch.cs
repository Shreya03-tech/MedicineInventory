using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineInventory.Models;

public class MedicineBatch : IValidatableObject
{
    public int Id { get; set; }

    [Required]
    public int MedicineId { get; set; }

    public Medicine? Medicine { get; set; }

    [Required(ErrorMessage = "Batch number is required.")]
    [StringLength(50, MinimumLength = 1)]
    [Display(Name = "Batch Number")]
    public string BatchNumber { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
    [Display(Name = "Quantity in Stock")]
    public int Quantity { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Manufacturing Date")]
    public DateTime ManufacturingDate { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    [Display(Name = "Expiry Date")]
    public DateTime ExpiryDate { get; set; } = DateTime.Today.AddYears(1);

    [Range(typeof(decimal), "0", "99999999",
        ErrorMessage = "Purchase price cannot be negative or exceed 99,999,999.")]
    [Column(TypeName = "decimal(10,2)")]
    [Display(Name = "Purchase Price")]
    public decimal PurchasePrice { get; set; }

    [Range(typeof(decimal), "0", "99999999",
        ErrorMessage = "Selling price cannot be negative or exceed 99,999,999.")]
    [Column(TypeName = "decimal(10,2)")]
    [Display(Name = "Selling Price")]
    public decimal SellingPrice { get; set; }

    [Required(ErrorMessage = "Supplier name is required.")]
    [StringLength(100)]
    [Display(Name = "Supplier Name")]
    public string SupplierName { get; set; } = string.Empty;

    [NotMapped]
    public string ExpiryStatus
    {
        get
        {
            var today = DateTime.Today;
            var expiryDate = ExpiryDate.Date;

            if (expiryDate < today)
                return "Expired";

            if (expiryDate <= today.AddDays(30))
                return "Expiring Soon";

            if (expiryDate <= today.AddDays(90))
                return "Expiring Within 90 Days";

            return "Safe";
        }
    }

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