using GymTrack.Areas.Identity.Data;
using GymTrack.Data;
using GymTrack.Models;
using GymTrack.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Diagnostics;

namespace GymTrack.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> Logger;
        private readonly UserManager<GymUser> UserManager;
        private readonly GymDbContext Context;
        public HomeController(ILogger<HomeController> logger, UserManager<GymUser> userManager, GymDbContext context)
        {
            Context = context;
            UserManager = userManager;
            Logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["UserID"] = UserManager.GetUserId(this.User);

            int month = Request.Query["month"].Count > 0 ? int.Parse(Request.Query["month"]) : DateTime.Now.Month;
            int year = Request.Query["year"].Count > 0 ? int.Parse(Request.Query["year"]) : DateTime.Now.Year;

            DateTime currentDate = new DateTime(year, month, 1);

            var user = await UserManager.GetUserAsync(User);
            var trainings = await Context.Trainings
                .Where(t => t.GymUserId == user.Id &&
                            t.Date.Month == currentDate.Month &&
                            t.Date.Year == currentDate.Year)
                .ToListAsync();

            var model = new HomeViewModel
            {
                currentDate = currentDate,
                trainingDays = trainings.Select(t => t.Date.Date).ToList()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
