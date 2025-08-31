using GymTrack.Interfaces;
using GymTrack.Models.DTOs;

namespace GymTrack.Services
{
    public class HomeService : IHomeService
    {
        private readonly IUserContext _userContext;
        private readonly ITrainingRepository _trainingRepository;
        public HomeService(IUserContext userContext, ITrainingRepository trainingRepository)
        {
            _userContext = userContext;
            _trainingRepository = trainingRepository;
        }

        public async Task<HomeDto> GetHomeDataAsync(int month, int year)
        {
            var userId = _userContext.GetUserId();
            var trainings = await _trainingRepository.GetTrainingsForMonthAsync(userId, month, year);
            DateTime currentDate = new DateTime(year, month, 1);

            return new HomeDto
            {
                CurrentDate = currentDate,
                UserId = userId,
                Trainingdays = trainings.Select(t => t.Date).ToList(),
            };
        }
    }
}