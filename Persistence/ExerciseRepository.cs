using GymTrack.Interfaces;
using GymTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace GymTrack.Persistence
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly GymDbContext GymDbContext;
        public ExerciseRepository(GymDbContext gymDbContext)
        {
            GymDbContext = gymDbContext;
        }

        public async Task<Exercise?> GetByNameAsync(string name)
        {
            return await GymDbContext.Exercise.FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task AddExerciseAsync(Exercise exercise)
        {
            await GymDbContext.Exercise.AddAsync(exercise);
        }
    }
}
