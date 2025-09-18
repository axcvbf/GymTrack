using AutoMapper;
using GymTrack.Application.DTOs;
using GymTrack.ViewModels;

namespace GymTrack.Application.Mappings
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
