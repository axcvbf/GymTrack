using GymTrack.Application.Interfaces;
using GymTrack.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymTrack.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            var result = await _accountService.LoginAsync(email, password, rememberMe);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string email, string password)
        {
            var result = await _accountService.RegisterAsync(email, password);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Login");
        }
    }
}

