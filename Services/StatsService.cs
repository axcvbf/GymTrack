using GymTrack.Interfaces;
using GymTrack.Models.DTOs;

namespace GymTrack.Services
{
    public class StatsService : IStatsService
    {
        private readonly ITrainingRepository TrainingRepository;
        public StatsService(ITrainingRepository trainingRepository)
        {
            TrainingRepository = trainingRepository;
        }
        public async Task<UserStatsDto> GetUserStatsAsync(string userId)
        {
            var trainings = await TrainingRepository.GetAllTrainingsForUserAsync(userId);
            var exercises = trainings.SelectMany(t => t.Exercises).ToList();

            return new UserStatsDto
            {
                TrainingsCount = trainings.Count(),
                SetsCount = exercises.Count(),
                RepsCount = exercises.Sum(e => e.Reps),
                TotalWeight = exercises.Sum(e => e.Weight * e.Reps)
            };
        }

        public async Task<List<ExerciseProgressDTO>>GetExerciseProgressAsync(string userId, int exerciseId)
        {
            var trainings = await TrainingRepository.GetAllTrainingsForUserAsync(userId);

            return trainings
                .SelectMany(t => t.Exercises)
                .Where(e => e.ExerciseId == exerciseId)
                .OrderBy(e => e.Training.Date)
                .Select(e => new ExerciseProgressDTO
                {
                    Date = e.Training.Date,
                    Weight = e.Weight
                })
                .ToList();
        }
    }
}
