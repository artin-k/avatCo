using avatCo.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register your DbContext as before
builder.Services.AddDbContext<AvatDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(cs, new MySqlServerVersion(new Version(8, 0, 39)));
});

// Add MVC (controllers + views)
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Enable attribute-based routing
app.MapControllers();

// Conventional area route (fallback for controller/action/id patterns)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();