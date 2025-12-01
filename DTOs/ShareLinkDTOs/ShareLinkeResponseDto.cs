namespace FileSharingPlatform.DTOs.ShareLinkDTOs
{
    public class ShareLinkeResponseDto
    {
        public Guid ShareLinkId { get; set; }
        public string ShareUrl { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
