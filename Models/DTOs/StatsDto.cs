using GymTrack.Persistence;

namespace GymTrack.Models.DTOs
{
    public class StatsDto
    {
        public UserStatsDto UserStats { get; set; }
        public List<ExerciseProgressDto> Benchpress { get; set; }
        public List<ExerciseProgressDto> Incline { get; set; }
        public List<ExerciseProgressDto> Shoulderpress { get; set; }
    }
}
