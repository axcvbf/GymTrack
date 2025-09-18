namespace GymTrack.Domain.Entities
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        public ICollection<ExerciseData> Datas { get; set; }
    }
}
