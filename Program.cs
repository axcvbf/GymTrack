using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GymTrack.Areas.Identity.Data;
using GymTrack.Persistence;
using GymTrack.Interfaces;
using GymTrack.Services;
using GymTrack.Mappings;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("GymDbContextConnection") ?? throw new InvalidOperationException("Connection string 'GymDbContextConnection' not found.");

builder.Services.AddDbContext<GymDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddDefaultIdentity<GymUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<GymDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(typeof(TrainingProfile));
builder.Services.AddAutoMapper(typeof(HomeProfile));
builder.Services.AddAutoMapper(typeof(StatsProfile));
builder.Services.AddScoped<ITrainingRepository, TrainingRepository>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<ITrainingService, TrainingService>();
builder.Services.AddScoped<IStatsService, StatsService>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IHomeService, HomeService>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
});

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
