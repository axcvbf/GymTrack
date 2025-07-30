using GymTrack.Areas.Identity.Data;
using GymTrack.Data;
using GymTrack.Models;
using GymTrack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GymTrack.Controllers
{
    public class TrainingController : Controller
    {
        private readonly UserManager<GymUser> UserManager;
        private readonly GymDbContext Context;

        public TrainingController(UserManager<GymUser> userManager, GymDbContext context)
        {
            UserManager = userManager;
            Context = context;
        }
        public IActionResult Create(DateTime date)
        {
            ViewData["UserID"] = UserManager.GetUserId(this.User);

            var model = new TrainingViewModel
            {
                Date = date,
                Exercises = new List<ExerciseViewModel>
                {
                    new ExerciseViewModel()
                }
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TrainingViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await UserManager.GetUserAsync(User);

            var training = new Training
            {
                Date = model.Date,
                GymUserId = user.Id,
                Exercises = new List<ExerciseData>()
            };

            foreach (var ex in model.Exercises)
            {
                var exercise = Context.Exercise.FirstOrDefault(e => e.Name == ex.Name);

                if(exercise == null)
                {
                    exercise = new Exercise { Name = ex.Name, Category = ex.Category };
                    Context.Exercise.Add(exercise);
                    await Context.SaveChangesAsync();
                }

                var exerciseData = new ExerciseData
                {
                    ExerciseId = exercise.Id,
                    Weight = ex.Weight,
                    Reps = ex.Reps,
                    Sets = ex.Sets
                };

                training.Exercises.Add(exerciseData);     
            }
            Context.Trainings.Add(training);
            await Context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
