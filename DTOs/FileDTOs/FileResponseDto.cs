namespace FileSharingPlatform.DTOs.FileDTOs
{
    public class FileResponseDto
    {
        public Guid FileId { get; set; }
        public string Name { get; set; } = string.Empty;
        public long Size { get; set; }
        public string MimeType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string FolderPath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }
}
