using GymTrack.Models.DTOs;

namespace GymTrack.Interfaces
{
    public interface IStatsService
    {
        Task<UserStatsDto> GetUserStatsAsync(string userId);
        Task<List<ExerciseProgressDTO>> GetExerciseProgressAsync(string userId, int exerciseId);
    }
}
