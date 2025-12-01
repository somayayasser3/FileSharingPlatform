using FileSharingPlatform.DTOs.UserDTOs;

namespace FileSharingPlatform.Services
{
    public interface IUserService
    {
        Task<UserProfileDto> GetUserProfileAsync(Guid userId);
    }
}
