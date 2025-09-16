using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GymTrack.Areas.Identity.Data;
using GymTrack.Persistence;
using GymTrack.Interfaces;
using GymTrack.Services;
using GymTrack.Mappings;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("GymDbContextConnection") ?? throw new InvalidOperationException("Connection string 'GymDbContextConnection' not found.");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("Logs/all_log.txt", rollingInterval: RollingInterval.Day)

    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("Business"))
        .WriteTo.File("Logs/business_log.txt", rollingInterval: RollingInterval.Day))

    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddDbContext<GymDbContext>(options => options.UseNpgsql(connectionString, o => o.EnableRetryOnFailure()));
builder.Services.AddDefaultIdentity<GymUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<GymDbContext>();
builder.Services.AddControllersWithViews(
    options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}
);
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
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
    var retries = 10;
    var delay = TimeSpan.FromSeconds(3);

    for(int i = 0; i < retries; i++)
    {
        try
        {
            dbContext.Database.Migrate();
            break;
        }
        catch (Npgsql.NpgsqlException)
        {
           System.Threading.Thread.Sleep(delay);
        }
    }
}

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
