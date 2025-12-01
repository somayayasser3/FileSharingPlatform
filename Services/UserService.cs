using FileSharingPlatform.Data.UnitOfWork;
using FileSharingPlatform.DTOs.UserDTOs;

namespace FileSharingPlatform.Services
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            return new UserProfileDto
            {
                UserId = user.UserId,
                Email = user.Email,
                FullName = user.FullName,
                StorageUsed = user.StorageUsed,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
