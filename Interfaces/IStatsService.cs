using GymTrack.Models.DTOs;

namespace GymTrack.Interfaces
{
    public interface IStatsService
    {
        Task<StatsDto> GetStatsDataAsync();
        Task<UserStatsDto> GetUserStatsAsync();
        Task<List<ExerciseProgressDto>> GetExerciseProgressAsync( int exerciseId);
    }
}
