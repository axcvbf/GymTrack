using GymTrack.Application.DTOs;
using GymTrack.ViewModels;

namespace GymTrack.Application.Interfaces
{
    public interface ITrainingService
    {
        Task<TrainingDto> GetTrainingDataAsync(DateTime date);
        Task SaveTrainingAsync(TrainingDto model);
    }
}
