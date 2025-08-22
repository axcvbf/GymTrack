using GymTrack.Areas.Identity.Data;
using GymTrack.Models;

namespace GymTrack.Interfaces
{
    public interface ITrainingRepository
    {
        Task <IEnumerable<Training>>GetAllTrainingsForUserAsync(string userId);
        Task<Training> GetTrainingAsync(string userId, DateTime Date);
        Task<IEnumerable<Training>> GetTrainingsForMonthAsync(string userId, int month, int year);
        Task AddTrainingAsync(Training training);
        Task UpdateTrainingAsync(Training training);
        Task DeleteTrainingAsync(Training training);
        Task SaveChangesAsync();
    }
}
