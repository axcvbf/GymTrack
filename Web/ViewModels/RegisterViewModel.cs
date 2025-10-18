using System.ComponentModel.DataAnnotations;

namespace GymTrack.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Must be identical to password field")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
