using Microsoft.EntityFrameworkCore;
using Mercato.Service;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=HBBTNRL0080\\TONY;Database=mercato;User Id=sa;Password=hojlund2004;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;")); // Remplacez par votre vraie chaîne de connexion

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews(); // Add this line for MVC
builder.Services.AddScoped<ClubAuthenticationService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IDbConnection>((sp) => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Add this line for MVC routing

app.Run();