using AutoMapper;
using GymTrack.Areas.Identity.Data;
using GymTrack.Interfaces;
using GymTrack.Models;
using GymTrack.Models.DTOs;
using GymTrack.Persistence;
using GymTrack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
