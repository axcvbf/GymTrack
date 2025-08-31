using GymTrack.Interfaces;
using GymTrack.Models;
using GymTrack.Models.DTOs;
using GymTrack.Persistence;
using Microsoft.AspNetCore.Identity;

namespace GymTrack.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository _trainingRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUserContext _userContext;
        public TrainingService(ITrainingRepository trainingRepository, IExerciseRepository exerciseRepository, IUserContext userContext)
        {
            _trainingRepository = trainingRepository;
            _exerciseRepository = exerciseRepository;
            _userContext = userContext;
        }

        public async Task<TrainingDto> GetTrainingDataAsync(DateTime date)
        {
            var userId = _userContext.GetUserId();
            var training = await _trainingRepository.GetTrainingAsync(userId, date);

            if (training != null)
            {
                return new TrainingDto
                {
                    Date = training.Date,
                    Exercises = training.Exercises.Select(e => new ExerciseDto
                    {
                        Name = e.Exercise.Name,
                        Category = e.Exercise.Category,
                        Weight = e.Weight,
                        Reps = e.Reps,
                    }).ToList()
                };
            }

            return new TrainingDto
            {
                Date = date,
                Exercises = new List<ExerciseDto>()
            };
        }

        public async Task SaveTrainingAsync(TrainingDto model)
        {
            var userId = _userContext.GetUserId();
            var existingTraining = await _trainingRepository.GetTrainingAsync(userId, model.Date);

            if (model.Exercises == null || !model.Exercises.Any())
            {
                if (existingTraining != null)
                {

                    await _trainingRepository.DeleteTrainingAsync(existingTraining);
                    await _trainingRepository.SaveChangesAsync();
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
                    var exercise = await _exerciseRepository.GetByNameAsync(ex.Name);

                    if (exercise == null)
                    {
                        exercise = new Exercise { Name = ex.Name, Category = ex.Category };
                        await _exerciseRepository.AddExerciseAsync(exercise);
                        await _trainingRepository.SaveChangesAsync();
                    }

                    training.Exercises.Add(new ExerciseData
                    {
                        ExerciseId = exercise.Id,
                        Weight = ex.Weight,
                        Reps = ex.Reps,
                    });
                }
                await _trainingRepository.AddTrainingAsync(training);
            }
            else
            {
                existingTraining.Exercises.Clear();


                foreach (var ex in model.Exercises)
                {
                    var exercise = await _exerciseRepository.GetByNameAsync(ex.Name);

                    if (exercise == null)
                    {
                        exercise = new Exercise { Name = ex.Name, Category = ex.Category };
                        await _exerciseRepository.AddExerciseAsync(exercise);
                        await _trainingRepository.SaveChangesAsync();
                    }

                    existingTraining.Exercises.Add(new ExerciseData
                    {
                        ExerciseId = exercise.Id,
                        Weight = ex.Weight,
                        Reps = ex.Reps,
                    });
                }
                await _trainingRepository.UpdateTrainingAsync(existingTraining);
            }

            await _trainingRepository.SaveChangesAsync();
        }
    }
}
