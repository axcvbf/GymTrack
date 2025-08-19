using GymTrack.Areas.Identity.Data;
using GymTrack.Data;
using GymTrack.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymTrack.Controllers
{
    public class StatsController : Controller
    {
        private readonly UserManager<GymUser> UserManager;
        private readonly GymDbContext Context;
        public StatsController(UserManager<GymUser> userManager, GymDbContext context)
        {
            UserManager = userManager;
            Context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated) return View(null);

            var userId = UserManager.GetUserId(this.User);
            ViewData["UserId"] = userId;

            var stats = await Context.Trainings
                .Where(t => t.GymUserId == userId)
                .SelectMany(t => t.Exercises)
                .GroupBy(e => 1)
                .Select(g => new UserStatsDto
                {
                    TrainingsCount = Context.Trainings.Count(t => t.GymUserId == userId),
                    SetsCount = g.Count(),
                    RepsCount = g.Sum(e => e.Reps),
                    TotalWeight = g.Sum(e => e.Reps * e.Weight)

                })
                .FirstOrDefaultAsync();

            return View(stats);
        }
    }
}
