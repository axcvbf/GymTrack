using GymTrack.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace GymTrack.Interfaces
{
    public interface IAccountService
    {
        Task<SignInResult> LoginAsync(string email, string password, bool rememberMe);
        Task<IdentityResult> RegisterAsync(string email, string password);
        Task LogoutAsync();
    }
}
