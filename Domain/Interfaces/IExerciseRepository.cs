using GymTrack.Domain.Entities;

namespace GymTrack.Domain.Interfaces
{
    public interface IExerciseRepository
    {
        Task<Exercise> GetByNameAsync(string name);
        Task AddExerciseAsync(Exercise exercise);

    }
}
