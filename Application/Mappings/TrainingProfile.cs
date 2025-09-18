using AutoMapper;
using GymTrack.Application.DTOs;
using GymTrack.ViewModels;

namespace GymTrack.Application.Mappings
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
