using GymTrack.Models;

namespace GymTrack.Interfaces
{
    public interface IExerciseRepository
    {
        Task<Exercise> GetByNameAsync(string name);
        Task AddExerciseAsync(Exercise exercise);

    }
}
