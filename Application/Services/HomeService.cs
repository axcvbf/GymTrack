using GymTrack.Application.DTOs;
using GymTrack.Application.Interfaces;
using GymTrack.Domain.Interfaces;
using Serilog;

namespace GymTrack.Application.Services
{
    public class HomeService : IHomeService
    {
        private readonly IUserContext _userContext;
        private readonly ITrainingRepository _trainingRepository;
        private readonly ILogger<HomeService> _logger;
        public HomeService(IUserContext userContext, ITrainingRepository trainingRepository, ILogger<HomeService> logger)
        {
            _userContext = userContext;
            _trainingRepository = trainingRepository;
            _logger = logger;
        }

        public async Task<HomeDto> GetHomeDataAsync(int month, int year)
        {
            var userId = _userContext.GetUserId();
            DateTime currentDate = new DateTime(year, month, 1);
            try
            {
                var trainings = await _trainingRepository.GetTrainingsForMonthAsync(userId, month, year);

                Log.ForContext("Business", true)
                    .Information("User {UserId} loaded calendar for month: {Month} year: {Year}", userId, month, year);

                return new HomeDto
                {
                    CurrentDate = currentDate,
                    UserId = userId,
                    Trainingdays = trainings.Select(t => t.Date).ToList(),
                };
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to load calendar for {UserId} on {Date}", userId, currentDate);

                Log.ForContext("Business", true)
                    .Warning("User {UserId} attempted to load calendar for {Date}, but an error occured", userId, currentDate);

                throw;
            }
        }
    }
}