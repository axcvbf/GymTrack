using GymTrack.Areas.Identity.Data;
using GymTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Diagnostics;

namespace GymTrack.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<GymUser> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<GymUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["UserID"] = _userManager.GetUserId(this.User);

            int month = Request.Query["month"].Count > 0 ? int.Parse(Request.Query["month"]) : DateTime.Now.Month;
            int year = Request.Query["year"].Count > 0 ? int.Parse(Request.Query["year"]) : DateTime.Now.Year;

            DateTime currentMonth = new DateTime(year, month, 1);

            ViewBag.Month = currentMonth;
            return View();
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
