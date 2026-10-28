
using MedicineInventory.Models;

namespace MedicineInventory.Models;

public class DashboardViewModel
{
    public int TotalMedicines { get; set; }
    public int ExpiredMedicines { get; set; }
    public int ExpiringSoonMedicines { get; set; }
    public int ExpiringWithin90DaysMedicines { get; set; }
    public int SafeMedicines { get; set; }
    public int LowStockMedicines { get; set; }
    public int NormalStockMedicines { get; set; }

    public List<Medicine> RecentMedicines { get; set; } = new();
}
