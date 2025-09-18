using AutoMapper;
using GymTrack.Application.DTOs;
using GymTrack.ViewModels;

namespace GymTrack.Application.Mappings
{
    public class HomeProfile : Profile
    {
        public HomeProfile()
        {
            CreateMap<HomeDto, HomeViewModel>()
                .ReverseMap();

            CreateMap<HomeViewModel, HomeDto>()
                .ReverseMap();
        }
    }
}
