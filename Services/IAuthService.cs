using FileSharingPlatform.DTOs.UserDTOs;

namespace FileSharingPlatform.Services
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    }
}
