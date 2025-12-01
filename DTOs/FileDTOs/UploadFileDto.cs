using System.ComponentModel.DataAnnotations;

namespace FileSharingPlatform.DTOs.FileDTOs
{
    public class UploadFileDto
    {
        [Required]
        public IFormFile File { get; set; } = null!;

        [Required]
        public Guid FolderId { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
