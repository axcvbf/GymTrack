using GymTrack.Application.Interfaces;
using GymTrack.Application.Services;
using GymTrack.ViewModels;
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"MODELSTATE ERROR: {error.ErrorMessage}");
                }

                return View();
            }
            var result = await _accountService.LoginAsync(model.Email, model.Password, model.RememberMe);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"MODELSTATE ERROR: {error.ErrorMessage}");
                }

                return View();
            }
            var result = await _accountService.RegisterAsync(model.Email, model.Password);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Login");
        }
    }
}

