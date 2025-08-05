using System.ComponentModel.DataAnnotations;

namespace GymTrack.ViewModels
{
    public class ExerciseViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Range(1,500)]
        public double Weight { get; set; }
        [Range(1, 50)]
        public int Reps { get; set; }
        //[Range(1, 50)]
        //public int Sets { get; set; }
        
    }
}
