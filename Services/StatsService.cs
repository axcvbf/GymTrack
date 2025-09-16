using GymTrack.Interfaces;
using GymTrack.Models.DTOs;
using Serilog;

namespace GymTrack.Services
{
    public class StatsService : IStatsService
    {
        private readonly ITrainingRepository _trainingRepository;
        private readonly IUserContext _userContext;
        private readonly ILogger<StatsService> _logger;
        public StatsService(ITrainingRepository trainingRepository, IUserContext userContext, ILogger<StatsService> logger)
        {
            _trainingRepository = trainingRepository;
            _userContext = userContext;
            _logger = logger;
        }

        public async Task<StatsDto> GetStatsDataAsync()
        {
            return new StatsDto
            {
                UserStats = await GetUserStatsAsync(),
                Benchpress = await GetExerciseProgressAsync(1),
                Incline = await GetExerciseProgressAsync(2),
                Shoulderpress = await GetExerciseProgressAsync(3),
            };
        }

        public async Task<UserStatsDto> GetUserStatsAsync()
        {
            var userId = _userContext.GetUserId();
            try
            {
                var trainings = await _trainingRepository.GetAllTrainingsForUserAsync(userId);
                var exercises = trainings.SelectMany(t => t.Exercises).ToList();

                Log.ForContext("Business", true)
                    .Information("User {UserId} requested user stats information", userId);

                return new UserStatsDto
                {
                    TrainingsCount = trainings.Count(),
                    SetsCount = exercises.Count(),
                    RepsCount = exercises.Sum(e => e.Reps),
                    TotalWeight = exercises.Sum(e => e.Weight * e.Reps)
                };
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Failed to load stats for user {UserId}", userId);
                Log.ForContext("Business", true)
                    .Warning("User {UserId} requested stats but an error occured", userId);

                throw;
            }
        }

        public async Task<List<ExerciseProgressDto>>GetExerciseProgressAsync(int exerciseId)
        {
            var userId = _userContext.GetUserId() ;
            var trainings = await _trainingRepository.GetAllTrainingsForUserAsync(userId);

            return trainings
                .SelectMany(t => t.Exercises)
                .Where(e => e.ExerciseId == exerciseId)
                .OrderBy(e => e.Training.Date)
                .Select(e => new ExerciseProgressDto
                {
                    Date = e.Training.Date,
                    Weight = e.Weight
                })
                .ToList();
        }
    }
}
