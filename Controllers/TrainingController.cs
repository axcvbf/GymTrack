using GymTrack.Areas.Identity.Data;
using GymTrack.Data;
using GymTrack.Models;
using GymTrack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Create(DateTime date)
        {
            var userId = UserManager.GetUserId(this.User);
            ViewData["UserID"] = userId;

            var existingTraining = await Context.Trainings
                .Include(t => t.Exercises)
                    .ThenInclude(e => e.Exercise)
                .FirstOrDefaultAsync(t => t.GymUserId == userId && t.Date == date.Date);

            if (existingTraining != null)
            {
                var model = new TrainingViewModel
                {
                    Date = existingTraining.Date,
                    Exercises = existingTraining.Exercises.Select(e => new ExerciseViewModel
                    {
                        Name =  e.Exercise.Name,
                        Weight = e.Weight,
                        Reps = e.Reps,
                        Sets = e.Sets
                    }).ToList()
                };

                return View(model);
            }


            return View(new TrainingViewModel
            {
                Date = date,
                Exercises = new List<ExerciseViewModel>()
            });
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
