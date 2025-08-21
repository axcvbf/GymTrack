using GymTrack.Models.DTOs;

namespace GymTrack.ViewModels
{
    public class StatsViewModel
    {
        public UserStatsDto UserStats { get; set; }
        public List<ExerciseProgressDTO> Benchpress { get; set; }
        public List<ExerciseProgressDTO> Incline { get; set; }
        public List<ExerciseProgressDTO> Shoulderpress { get; set; }
    }
}
