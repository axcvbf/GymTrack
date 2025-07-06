using GymTrack.Areas.Identity.Data;
using GymTrack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymTrack.Controllers
{
    public class TrainingController : Controller
    {
        private readonly UserManager<GymUser> UserManager;

        public TrainingController(UserManager<GymUser> userManager)
        {
            UserManager = userManager;
        }
        public IActionResult Create(DateTime date)
        {
            ViewData["UserID"] = UserManager.GetUserId(this.User);

            var model = new TrainingViewModel
            {
                Date = date,
                Excercises = new List<ExcerciseViewModel>
                {
                    new ExcerciseViewModel()
                }
            };


            return View(model);
        }

        [HttpPost]
        public IActionResult Create(TrainingViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}
