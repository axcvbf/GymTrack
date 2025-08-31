using AutoMapper;
using GymTrack.Models.DTOs;
using GymTrack.ViewModels;

namespace GymTrack.Mappings
{
    public class StatsProfile : Profile
    {
        public StatsProfile() 
        {
            CreateMap<StatsDto, StatsViewModel>()
                .ReverseMap();

            CreateMap<StatsViewModel, StatsDto>()
                .ReverseMap();
        }
    }
}
