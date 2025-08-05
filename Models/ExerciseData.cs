namespace GymTrack.Models
{
    public class ExerciseData
    {
        public int Id { get; set; }
        public double Weight { get; set; }
        public int Reps { get; set; }
        //public int Sets { get; set; }
        public int TrainingId { get; set; }
        public Training Training { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }

    }
}
