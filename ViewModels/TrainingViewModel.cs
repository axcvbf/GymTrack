using System.ComponentModel.DataAnnotations;

namespace GymTrack.ViewModels
{
    public class TrainingViewModel
    {
        [Required]
        public DateTime Date { get; set; }
        public List<ExerciseViewModel> Exercises{ get; set; }
    }
}
