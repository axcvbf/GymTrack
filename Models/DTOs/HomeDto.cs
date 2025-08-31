namespace GymTrack.Models.DTOs
{
    public class HomeDto
    {
        public DateTime CurrentDate { get; set; }
        public List<DateTime> Trainingdays { get; set; }
        public string UserId { get; set; }
    }
}
