namespace FileSharingPlatform.DTOs.FoldersAndFilesListingDTOs
{
    public class FolderItemsResponseDto // For Paginated Items Response
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public FolderItemDto[] Items { get; set; } = Array.Empty<FolderItemDto>();
    }
}
