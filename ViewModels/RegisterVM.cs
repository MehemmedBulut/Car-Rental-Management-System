using System.ComponentModel.DataAnnotations;

namespace RentalFinal.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage ="Bu xana boş ola bilməz!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string CheckPassword { get; set; }
        public bool IsRemember { get; set; }
    }
}
