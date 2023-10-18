using System.ComponentModel.DataAnnotations;

namespace RentalFinal.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Email tələb olunur.")]
        [EmailAddress(ErrorMessage = "Zəhmət olmasa düzgün email qeyd edin.")]
        public string Email { get; set; }
    }
}
