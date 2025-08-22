using GymTrack.Interfaces;
using GymTrack.Models;
using GymTrack.Persistence;
using GymTrack.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace GymTrack.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository TrainingRepository;
        private readonly IExerciseRepository ExerciseRepository;
        public TrainingService(ITrainingRepository trainingRepository, IExerciseRepository exerciseRepository)
        {
            TrainingRepository = trainingRepository;
            ExerciseRepository = exerciseRepository;
        }

        public async Task<TrainingViewModel> GetTrainingViewModelAsync(string userId, DateTime date)
        {
            var training = await TrainingRepository.GetTrainingAsync(userId, date);

            if (training != null)
            {
                return new TrainingViewModel
                {
                    Date = training.Date,
                    Exercises = training.Exercises.Select(e => new ExerciseViewModel
                    {
                        Name = e.Exercise.Name,
                        Category = e.Exercise.Category,
                        Weight = e.Weight,
                        Reps = e.Reps,
                    }).ToList()
                };
            }

            return new TrainingViewModel
            {
                Date = date,
                Exercises = new List<ExerciseViewModel>()
            };
        }

        public async Task SaveTrainingAsync(string userId, TrainingViewModel model)
        {
            var existingTraining = await TrainingRepository.GetTrainingAsync(userId, model.Date);

            if (model.Exercises == null || !model.Exercises.Any())
            {
                if (existingTraining != null)
                {

                    await TrainingRepository.DeleteTrainingAsync(existingTraining);
                    await TrainingRepository.SaveChangesAsync();
                }
                return;

            }

            if (existingTraining == null)
            {
                var training = new Training
                {
                    Date = model.Date,
                    GymUserId = userId,
                    Exercises = new List<ExerciseData>()
                };

                foreach (var ex in model.Exercises)
                {
                    var exercise = await ExerciseRepository.GetByNameAsync(ex.Name);

                    if (exercise == null)
                    {
                        exercise = new Exercise { Name = ex.Name, Category = ex.Category };
                        await ExerciseRepository.AddExerciseAsync(exercise);
                        await TrainingRepository.SaveChangesAsync();
                    }

                    training.Exercises.Add(new ExerciseData
                    {
                        ExerciseId = exercise.Id,
                        Weight = ex.Weight,
                        Reps = ex.Reps,
                    });
                }
                await TrainingRepository.AddTrainingAsync(training);
            }
            else
            {
                existingTraining.Exercises.Clear();


                foreach (var ex in model.Exercises)
                {
                    var exercise = await ExerciseRepository.GetByNameAsync(ex.Name);

                    if (exercise == null)
                    {
                        exercise = new Exercise { Name = ex.Name, Category = ex.Category };
                        await ExerciseRepository.AddExerciseAsync(exercise);
                        await TrainingRepository.SaveChangesAsync();
                    }

                    existingTraining.Exercises.Add(new ExerciseData
                    {
                        ExerciseId = exercise.Id,
                        Weight = ex.Weight,
                        Reps = ex.Reps,
                    });
                }
                await TrainingRepository.UpdateTrainingAsync(existingTraining);
            }

            await TrainingRepository.SaveChangesAsync();
        }
    }
}
