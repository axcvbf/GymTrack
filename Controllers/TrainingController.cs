using GymTrack.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymTrack.Controllers
{
    public class TrainingController : Controller
    {
        public IActionResult Create(DateTime date)
        {
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
