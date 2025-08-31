using GymTrack.Models.DTOs;
using GymTrack.ViewModels;

namespace GymTrack.Interfaces
{
    public interface ITrainingService
    {
        Task<TrainingDto> GetTrainingDataAsync(DateTime date);
        Task SaveTrainingAsync(TrainingDto model);
    }
}
