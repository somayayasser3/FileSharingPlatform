namespace FileSharingPlatform.Models
{
    public class FFile
    {
        public Guid FileId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StoragePath { get; set; } = string.Empty; 
        public long Size { get; set; }
        public string MimeType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid FolderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User User { get; set; } = null!;
        public Folder Folder { get; set; } = null!;
        public ICollection<PublicShareLink> ShareLinks { get; set; } = new List<PublicShareLink>();
    }
}
