using AutoMapper;
using GymTrack.Interfaces;
using GymTrack.Models.DTOs;
using GymTrack.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymTrack.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IMapper _mapper;

        public TrainingController(ITrainingService trainingService, IMapper mapper)
        {
            _mapper = mapper;
            _trainingService = trainingService;
        }
        public async Task<IActionResult> Index(DateTime date)
        {
            var dto = await _trainingService.GetTrainingDataAsync(date);
            var vm = _mapper.Map<TrainingViewModel>(dto);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Index(TrainingViewModel vm)
        {
            var dto = _mapper.Map<TrainingDto>(vm);
            await _trainingService.SaveTrainingAsync(dto);
            return RedirectToAction("Index", "Home");
        }
    }
}
