using GymTrack.Models.DTOs;

namespace GymTrack.Interfaces
{
    public interface IHomeService
    {
        Task<HomeDto> GetHomeDataAsync(int month, int year);
    }
}
