using GymTrack.Areas.Identity.Data;
using GymTrack.Interfaces;
using GymTrack.Models;
using GymTrack.Persistence;
using GymTrack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GymTrack.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ITrainingService TrainingService;
        private readonly UserManager<GymUser> UserManager;

        public TrainingController(ITrainingService trainingService, UserManager<GymUser> userManager)
        {
            TrainingService = trainingService;
            UserManager = userManager;
        }
        public async Task<IActionResult> Index(DateTime date)
        {
            var userId = UserManager.GetUserId(this.User);
            ViewData["UserID"] = userId;

            var model = await TrainingService.GetTrainingViewModelAsync(userId, date);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(TrainingViewModel model)
        {
            var userId = UserManager.GetUserId(User);

            await TrainingService.SaveTrainingAsync(userId, model);

            return RedirectToAction("Index", "Home");
        }
    }
}
