using GymTrack.Application.DTOs;

namespace GymTrack.Application.Interfaces
{
    public interface IHomeService
    {
        Task<HomeDto> GetHomeDataAsync(int month, int year);
    }
}
