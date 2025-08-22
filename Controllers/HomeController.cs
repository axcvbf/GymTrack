using GymTrack.Areas.Identity.Data;
using GymTrack.Interfaces;
using GymTrack.Models;
using GymTrack.Persistence;
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
        private readonly ITrainingRepository TrainingRepository;
        private readonly UserManager<GymUser> UserManager;

        public HomeController(ITrainingRepository trainingRepository, UserManager<GymUser> userManager)
        {
            TrainingRepository = trainingRepository;
            UserManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = UserManager.GetUserId(this.User);
            ViewData["UserID"] = userId;

            int month = Request.Query["month"].Count > 0 ? int.Parse(Request.Query["month"]) : DateTime.Now.Month;
            int year = Request.Query["year"].Count > 0 ? int.Parse(Request.Query["year"]) : DateTime.Now.Year;

            DateTime currentDate = new DateTime(year, month, 1);

            var trainings = await TrainingRepository.GetTrainingsForMonthAsync(userId, month, year);

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
