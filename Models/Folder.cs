namespace FileSharingPlatform.Models
{
    public class Folder
    {
        public Guid FolderId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? ParentFolderId { get; set; } // Nullable for root folders [recursive relationship]
        public Guid UserId { get; set; }
        public string Path { get; set; } = string.Empty;
        public int Depth { get; set; } = 0;  // prevents exceeding 10-level limit
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User User { get; set; } = null!;
        public Folder? ParentFolder { get; set; }
        public ICollection<Folder> SubFolders { get; set; } = new List<Folder>();
        public ICollection<FFile> Files { get; set; } = new List<FFile>();
        public ICollection<PublicShareLink> ShareLinks { get; set; } = new List<PublicShareLink>();
    }
}
