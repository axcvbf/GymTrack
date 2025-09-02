using AutoMapper;
using GymTrack.Interfaces;
using GymTrack.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymTrack.Controllers
{
    public class StatsController : Controller
    {
        private readonly IStatsService _statsService;
        private readonly IMapper _mapper;
        public StatsController(IStatsService statsService, IMapper mapper)
        {
            _statsService = statsService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated) 
                return View(null);

            var dto = await _statsService.GetStatsDataAsync();
            var vm = _mapper.Map<StatsViewModel>(dto);

            return View(vm);
        }
    }
}
