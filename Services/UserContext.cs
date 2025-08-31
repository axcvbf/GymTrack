using GymTrack.Areas.Identity.Data;
using GymTrack.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace GymTrack.Services
{
    public class UserContext : IUserContext
    {
        private readonly UserManager<GymUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContext(UserManager<GymUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = _userManager.GetUserId(user);

            return userId;
        }

    }
}
