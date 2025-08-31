using AutoMapper;
using GymTrack.Areas.Identity.Data;
using GymTrack.Interfaces;
using GymTrack.Models;
using GymTrack.Persistence;
using GymTrack.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Diagnostics;

namespace GymTrack.Controllers
{
    [Authorize]
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
