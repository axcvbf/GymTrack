using GymTrack.Domain.Entities;
using GymTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymTrack.Infrastructure.Persistence.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly GymDbContext _gymDbContext;
        public ExerciseRepository(GymDbContext gymDbContext)
        {
            _gymDbContext = gymDbContext;
        }

        public async Task<Exercise?> GetByNameAsync(string name)
        {
            return await _gymDbContext.Exercise.FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task AddExerciseAsync(Exercise exercise)
        {
            await _gymDbContext.Exercise.AddAsync(exercise);
        }
    }
}
