using GymTrack.Application.DTOs;

namespace GymTrack.ViewModels
{
    public class StatsViewModel
    {
        public UserStatsDto UserStats { get; set; }
        public List<ExerciseProgressDto> Benchpress { get; set; }
        public List<ExerciseProgressDto> Incline { get; set; }
        public List<ExerciseProgressDto> Shoulderpress { get; set; }
    }
}
