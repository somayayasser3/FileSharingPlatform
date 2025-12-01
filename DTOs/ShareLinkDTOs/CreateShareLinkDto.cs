using System.ComponentModel.DataAnnotations;

namespace FileSharingPlatform.DTOs.ShareLinkDTOs
{
    public class CreateShareLinkDto
    {
        [Range(1, 30)]
        public int ExpirationDays { get; set; } = 7;

        [MinLength(4)]
        public string? Password { get; set; }
    }
}
