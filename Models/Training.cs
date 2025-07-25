﻿using GymTrack.Areas.Identity.Data;

namespace GymTrack.Models
{
    public class Training
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string GymUserId { get; set; }
        public GymUser GymUser { get; set; }
        public ICollection<ExerciseData> Excercises { get; set; }
    }
}
