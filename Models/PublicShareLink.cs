namespace FileSharingPlatform.Models
{
    public class PublicShareLink
    {
        public Guid ShareLinkId { get; set; }
        public string Token { get; set; } = string.Empty; // 32-character unique token
        public Guid? FileId { get; set; }
        public Guid? FolderId { get; set; }
        public Guid UserId { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true; //allows manual disabling before expiration
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastAccessedAt { get; set; }

        // Navigation Properties
        public User User { get; set; } = null!;
        public FFile? File { get; set; }
        public Folder? Folder { get; set; }
    }
}