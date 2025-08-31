using GymTrack.Areas.Identity.Data;
using GymTrack.Interfaces;
using GymTrack.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GymTrack.Persistence
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly GymDbContext _gymDbContext;

        public TrainingRepository(GymDbContext gymDbContext)
        {
            _gymDbContext = gymDbContext;
        }
        public async Task<IEnumerable<Training>> GetAllTrainingsForUserAsync(string userId)
        {
            return await _gymDbContext.Trainings
                .Include(t => t.Exercises)
                .Where(t => t.GymUserId == userId)
                .ToListAsync();
        }
        public async Task<Training> GetTrainingAsync(string userId, DateTime date)
        {
            return await _gymDbContext.Trainings
                .Include(t => t.Exercises)
                    .ThenInclude(e => e.Exercise)
                .FirstOrDefaultAsync(t => t.GymUserId == userId && t.Date == date.Date);
        }

        public async Task<IEnumerable<Training>> GetTrainingsForMonthAsync(string userId, int month, int year)
        { 
            return await _gymDbContext.Trainings
                .Where(t => t.GymUserId == userId && 
                t.Date.Month == month &&
                t.Date.Year == year)
                .ToListAsync();

        }

        public async Task AddTrainingAsync(Training training)
        {
            await _gymDbContext.Trainings.AddAsync(training);
        }

        public Task UpdateTrainingAsync(Training training)
        {
            _gymDbContext.Trainings.Update(training);
            return Task.CompletedTask;
        }

        public Task DeleteTrainingAsync(Training training)
        {
            _gymDbContext.Trainings.Remove(training);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _gymDbContext.SaveChangesAsync();
        }
    }
}