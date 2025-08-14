using GymTrack.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymTrack.Controllers
{
    public class StatsController : Controller
    {
        private readonly UserManager<GymUser> UserManager;
        public StatsController(UserManager<GymUser> userManager)
        {
            UserManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = UserManager.GetUserId(this.User);
            ViewData["UserId"] = userId;
            return View();
        }
    }
}
