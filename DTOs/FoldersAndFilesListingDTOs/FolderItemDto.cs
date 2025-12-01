namespace FileSharingPlatform.DTOs.FoldersAndFilesListingDTOs
{
    public class FolderItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "Folder" or "File"
        public long? Size { get; set; } // Null for folders
        public DateTime ModifiedDate { get; set; }
    }
}
