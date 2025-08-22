using GymTrack.Areas.Identity.Data;
using GymTrack.Interfaces;
using GymTrack.Models;
using GymTrack.ViewModels;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GymTrack.Persistence
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly GymDbContext GymDbContext;

        public TrainingRepository(GymDbContext gymDbContext)
        {
            GymDbContext = gymDbContext;
        }
        public async Task<IEnumerable<Training>> GetAllTrainingsForUserAsync(string userId)
        {
            return await GymDbContext.Trainings
                .Include(t => t.Exercises)
                .Where(t => t.GymUserId == userId)
                .ToListAsync();
        }
        public async Task<Training> GetTrainingAsync(string userId, DateTime date)
        {
            return await GymDbContext.Trainings
                .Include(t => t.Exercises)
                    .ThenInclude(e => e.Exercise)
                .FirstOrDefaultAsync(t => t.GymUserId == userId && t.Date == date.Date);
        }

        public async Task<IEnumerable<Training>> GetTrainingsForMonthAsync(string userId, int month, int year)
        { 
            return await GymDbContext.Trainings
                .Where(t => t.GymUserId == userId && 
                t.Date.Month == month &&
                t.Date.Year == year)
                .ToListAsync();

        }

        public async Task AddTrainingAsync(Training training)
        {
            await GymDbContext.Trainings.AddAsync(training);
        }

        public Task UpdateTrainingAsync(Training training)
        {
            GymDbContext.Trainings.Update(training);
            return Task.CompletedTask;
        }

        public Task DeleteTrainingAsync(Training training)
        {
            GymDbContext.Trainings.Remove(training);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await GymDbContext.SaveChangesAsync();
        }
    }
}