using GymTrack.Application.Interfaces;
using GymTrack.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Serilog;

namespace GymTrack.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<GymUser> _userManager;
        private readonly SignInManager<GymUser> _signInManager;
        private readonly ILogger<AccountService> _logger;
        public AccountService(
            UserManager<GymUser> userManager,
            SignInManager<GymUser> signInManager,
            ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<SignInResult> LoginAsync(string email, string password, bool rememberMe)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            try
            {
                if (user == null)
                {
                    _logger.LogError("Failed to log in user, user null");

                    return SignInResult.Failed;
                }

                var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {UserId} succesfully logged in", user.Id);
                    Log.ForContext("Business", true)
                        .Warning("User {UserId} logged in", user.Id);
                }
                else if (result.IsLockedOut)
                {
                    _logger.LogWarning("User {UserId} tried to log in but its account is locked out", user.Id);
                }
                else
                {
                    _logger.LogWarning("Failed login attemdpt for user {UserId}", user.Id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while logging in {UserId}", user.Id);
                throw;
            }

        }

        public async Task<IdentityResult> RegisterAsync(string email, string password)
        {
            try
            {
                var user = new GymUser
                {
                    Email = email,
                    UserName = email,
                };
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);

                    _logger.LogInformation("User {UserId} registered successfully", user.Id);
                    Log.ForContext("Business", true)
                        .Information("User {UserId} registered successfully", user.Id);

                }
                else
                {
                    _logger.LogWarning("Failed registration attempt");
                    Log.ForContext("Business", true)
                        .Information("Failed registration attempt");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to register user");
                throw;
            }

        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            Log.ForContext("Business", true)
                .Information("User logged out");
        }
    }
}
