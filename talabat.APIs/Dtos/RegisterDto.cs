using System.ComponentModel.DataAnnotations;

namespace talabat.APIs.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&amp;*()_+]).*$",
            ErrorMessage = "Password must contains 1 uppercase, 1 lowercase, 1 digit, 1 special charachter")]
        public string Password { get; set; }
    }
}
