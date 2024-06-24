using Microsoft.EntityFrameworkCore;
using Mercato.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=HBBTNRL0080\\TONY;Database=mercato;User Id=sa;Password=hojlund2004;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;")); // Remplacez par votre vraie chaîne de connexion

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ClubAuthenticationService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
