namespace FileSharingPlatform.DTOs.ShareLinkDTOs
{
    public class ShareLinkDetailsDto
    {
        public string Type { get; set; } = string.Empty; // "file" or "folder"
        public string Name { get; set; } = string.Empty;
        public long? Size { get; set; }
        public string? MimeType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool RequiresPassword { get; set; }
    }
}
