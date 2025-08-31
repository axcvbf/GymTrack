using AutoMapper;
using GymTrack.Models.DTOs;
using GymTrack.ViewModels;

namespace GymTrack.Mappings
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
