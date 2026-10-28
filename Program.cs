
using MedicineInventory.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register MVC controllers and Razor Views.
builder.Services.AddControllersWithViews();

// Read the connection string from configuration/User Secrets.
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Database connection string 'DefaultConnection' was not configured.");

// Register the EF Core database context using Oracle's MySQL provider.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
