namespace GymTrack.Models.DTOs
{
    public class TrainingDto
    {
        public DateTime Date { get; set; }
        public List<ExerciseDto> Exercises { get; set; }
    }
}
