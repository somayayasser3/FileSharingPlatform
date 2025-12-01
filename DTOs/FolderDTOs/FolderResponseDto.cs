namespace FileSharingPlatform.DTOs.FolderDTOs
{
    public class FolderResponseDto
    {
        public Guid FolderId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        
        public Guid? ParentFolderId { get; set; }
        public int Depth { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
