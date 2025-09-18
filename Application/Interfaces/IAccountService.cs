using Microsoft.AspNetCore.Identity;

namespace GymTrack.Application.Interfaces
{
    public interface IAccountService
    {
        Task<SignInResult> LoginAsync(string email, string password, bool rememberMe);
        Task<IdentityResult> RegisterAsync(string email, string password);
        Task LogoutAsync();
    }
}
