using System.ComponentModel.DataAnnotations;

namespace GymTrack.Models
{
    public class TrainingViewModel
    {
        [Required]
        public DateTime Date { get; set; }
        public List<ExcerciseViewModel> Excercises{ get; set; }
    }
}
