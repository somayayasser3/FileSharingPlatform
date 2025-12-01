namespace FileSharingPlatform.Models
{
    public class User
    {
        public Guid UserId { get; set; }  // GUID for uniqueness
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public long StorageUsed { get; set; } = 0; // tracks total file size per user in bytes
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }

        // Navigation Properties
        public ICollection<Folder> Folders { get; set; } = new List<Folder>();
        public ICollection<FFile> Files { get; set; } = new List<FFile>();
        public ICollection<PublicShareLink> ShareLinks { get; set; } = new List<PublicShareLink>();
    }
}
