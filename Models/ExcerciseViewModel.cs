using System.ComponentModel.DataAnnotations;

namespace GymTrack.Models
{
    public class ExcerciseViewModel
    {
        [Required]
        public string Name { get; set; }
        [Range(1,500)]
        public double Weight { get; set; }
        [Range(1, 50)]
        public int Reps { get; set; }
        [Range(1, 50)]
        public int Sets { get; set; }
        
    }
}
