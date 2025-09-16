using GymTrack.Interfaces;
using GymTrack.Models;
using GymTrack.Models.DTOs;
using Serilog;

namespace GymTrack.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository _trainingRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUserContext _userContext;
        private readonly ILogger<TrainingService> _logger;
        public TrainingService(
            ITrainingRepository trainingRepository,
            IExerciseRepository exerciseRepository,
            IUserContext userContext,
            ILogger<TrainingService> logger)
        {
            _trainingRepository = trainingRepository;
            _exerciseRepository = exerciseRepository;
            _userContext = userContext;
            _logger = logger;
        }

        public ITrainingRepository TrainingRepository => _trainingRepository;

        public async Task<TrainingDto> GetTrainingDataAsync(DateTime date)
        {
            var userId = _userContext.GetUserId();
            try
            {
                var training = await TrainingRepository.GetTrainingAsync(userId, date);

                if (training != null)
                {
                    Log.ForContext("Business", true)
                         .Information("User {UserId} requested training for {Date}", userId, date);

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

                Log.ForContext("Business", true)
                    .Information("User {UserId} requested training for {Date}, but none exits", userId, date);

                return new TrainingDto
                {
                    Date = date,
                    Exercises = new List<ExerciseDto>()
                };
            }
            catch(Exception ex) {
                _logger.LogError(ex, "Error while retrieving training for {UserId} on {Date}", userId, date);

                Log.ForContext("Business", true)
                    .Warning("User {UserId} tried to retrieve training for {Date}, but an error occured", userId, date);

                throw;
            }
        }

        public async Task SaveTrainingAsync(TrainingDto model)
        {
            var userId = _userContext.GetUserId();
            try
            {
                var existingTraining = await TrainingRepository.GetTrainingAsync(userId, model.Date);

                if (model.Exercises == null || !model.Exercises.Any())
                {
                    if (existingTraining != null)
                    {
                        await TrainingRepository.DeleteTrainingAsync(existingTraining);
                        await TrainingRepository.SaveChangesAsync();

                        Log.ForContext("Business", true)
                            .Information("user {UserId} deleted training for {Date}", userId, model.Date);
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
                            await TrainingRepository.SaveChangesAsync();

                            Log.ForContext("Business", true)
                                .Information("User {UserId} added new exercise to training for {Date} - {ExerciseName}",
                                userId, model.Date, exercise.Name);
                        }

                        training.Exercises.Add(new ExerciseData
                        {
                            ExerciseId = exercise.Id,
                            Weight = ex.Weight,
                            Reps = ex.Reps,
                        });
                    }
                    await TrainingRepository.AddTrainingAsync(training);

                    Log.ForContext("Business", true)
                        .Information("user {UserId} saved new training for {Date} with {ExerciseCount} exercises",
                        userId, model.Date, model.Exercises.Count);
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
                            await TrainingRepository.SaveChangesAsync();

                            Log.ForContext("Business", true)
                                .Information("User {UserId} added new exercise to training for {Date} - {ExerciseName}",
                                userId, model.Date, exercise.Name);
                        }

                        existingTraining.Exercises.Add(new ExerciseData
                        {
                            ExerciseId = exercise.Id,
                            Weight = ex.Weight,
                            Reps = ex.Reps,
                        });
                    }
                    await TrainingRepository.UpdateTrainingAsync(existingTraining);

                    Log.ForContext("Business", true)
                        .Information("User {UserId} saved new training for {Date} with {ExerciseCount} exercises",
                        userId, model.Date, model.Exercises.Count);
                }

                await TrainingRepository.SaveChangesAsync();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error while saving training for {UserId} on {Date}", userId, model.Date);

                Log.ForContext("Business", true)
                    .Warning("User {UserId} attempted to save training for {Date}, but an error occured", userId, model.Date);

                throw;
            }
        }
    }
}
