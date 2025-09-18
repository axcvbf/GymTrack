using GymTrack.Application.DTOs;

namespace GymTrack.Application.Interfaces
{
    public interface IStatsService
    {
        Task<StatsDto> GetStatsDataAsync();
        Task<UserStatsDto> GetUserStatsAsync();
        Task<List<ExerciseProgressDto>> GetExerciseProgressAsync(int exerciseId);
    }
}
