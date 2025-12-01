using System.ComponentModel.DataAnnotations;

namespace FileSharingPlatform.DTOs.UserDTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Password must contain at least 1 uppercase letter and 1 number")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; } = string.Empty;
    }
}
