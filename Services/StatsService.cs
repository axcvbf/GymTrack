using GymTrack.Interfaces;
using GymTrack.Models.DTOs;

namespace GymTrack.Services
{
    public class StatsService : IStatsService
    {
        private readonly ITrainingRepository _trainingRepository;
        private readonly IUserContext _userContext;
        public StatsService(ITrainingRepository trainingRepository, IUserContext userContext)
        {
            _trainingRepository = trainingRepository;
            _userContext = userContext;
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
            var trainings = await _trainingRepository.GetAllTrainingsForUserAsync(userId);
            var exercises = trainings.SelectMany(t => t.Exercises).ToList();

            return new UserStatsDto
            {
                TrainingsCount = trainings.Count(),
                SetsCount = exercises.Count(),
                RepsCount = exercises.Sum(e => e.Reps),
                TotalWeight = exercises.Sum(e => e.Weight * e.Reps)
            };
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
