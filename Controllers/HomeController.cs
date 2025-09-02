using AutoMapper;
using GymTrack.Interfaces;
using GymTrack.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymTrack.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly IMapper _mapper;
        public HomeController(IHomeService homeService, IMapper mapper)
        {
            _homeService = homeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int? month, int? year)
        {
            int m = month ?? DateTime.Now.Month;
            int y = year ?? DateTime.Now.Year;

            var dto = await _homeService.GetHomeDataAsync(m, y);
            var vm = _mapper.Map<HomeViewModel>(dto);
            return View(vm);
        }
    }
}
