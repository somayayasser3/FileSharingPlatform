using FileSharingPlatform.Data.UnitOfWork;
using FileSharingPlatform.DTOs.UserDTOs;
using FileSharingPlatform.Helpers;
using FileSharingPlatform.Models;

namespace FileSharingPlatform.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JWTHelper _jwtHelper;

        public AuthService(IUnitOfWork unitOfWork, JWTHelper jwtHelper)
        {
            _unitOfWork = unitOfWork;
            _jwtHelper = jwtHelper;
        }
        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            // Find user by email
            var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            // Generate JWT token
            var token = _jwtHelper.GenerateToken(user.UserId, user.Email);

            return new LoginResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                User = new UserProfileDto
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    FullName = user.FullName,
                    StorageUsed = user.StorageUsed,
                    CreatedAt = user.CreatedAt
                }
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Check if email already exists
            if (await _unitOfWork.Users.EmailExistsAsync(registerDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Create user
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = registerDto.Email.ToLower(),
                PasswordHash = passwordHash,
                FullName = registerDto.FullName,
                StorageUsed = 0,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new RegisterResponseDto
            {
                UserId = user.UserId,
                Email = user.Email
            };
        }


    }
}
