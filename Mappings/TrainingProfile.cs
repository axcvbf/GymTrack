using AutoMapper;
using GymTrack.Models.DTOs;
using GymTrack.ViewModels;

namespace GymTrack.Mappings
{
    public class TrainingProfile : Profile
    {
        public TrainingProfile()
        {
            CreateMap<TrainingViewModel, TrainingDto>()
                .ReverseMap();

            CreateMap<ExerciseViewModel, ExerciseDto>()
                .ReverseMap();
        }
    }
}
