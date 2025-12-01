namespace FileSharingPlatform.DTOs.UserDTOs
{
    public class UserProfileDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public long StorageUsed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
