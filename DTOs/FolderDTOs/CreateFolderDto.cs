using System.ComponentModel.DataAnnotations;

namespace FileSharingPlatform.DTOs.FolderDTOs
{
    public class CreateFolderDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public Guid? ParentFolderId { get; set; }
    }
}
