using GymTrack.Areas.Identity.Data;
using GymTrack.Interfaces;
using GymTrack.Models.DTOs;
using GymTrack.Persistence;
using GymTrack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymTrack.Controllers
{
    public class StatsController : Controller
    {
        private readonly UserManager<GymUser> UserManager;
        private readonly IStatsService StatsService;
        public StatsController(UserManager<GymUser> userManager, IStatsService statsService)
        {
            UserManager = userManager;
            StatsService = statsService;
        }
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated) 
                return View(null);

            var userId = UserManager.GetUserId(this.User);
            ViewData["UserId"] = userId;

            var stats = await StatsService.GetUserStatsAsync(userId);
            var bench = await StatsService.GetExerciseProgressAsync(userId, 1);
            var incline = await StatsService.GetExerciseProgressAsync(userId, 1);
            var shoulderpress = await StatsService.GetExerciseProgressAsync(userId, 1);

            var model = new StatsViewModel
            {
                UserStats = stats,
                Benchpress = bench,
                Incline = incline,
                Shoulderpress = shoulderpress
            };

            return View(model);
        }
    }
}
