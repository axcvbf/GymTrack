using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GymTrack.Data;
using GymTrack.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("GymDbContextConnection") ?? throw new InvalidOperationException("Connection string 'GymDbContextConnection' not found.");

builder.Services.AddDbContext<GymDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<GymUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<GymDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
