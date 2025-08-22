using GymTrack.ViewModels;

namespace GymTrack.Interfaces
{
    public interface ITrainingService
    {
        Task<TrainingViewModel> GetTrainingViewModelAsync(string userId, DateTime date);
        Task SaveTrainingAsync(string userId, TrainingViewModel model);
    }
}
