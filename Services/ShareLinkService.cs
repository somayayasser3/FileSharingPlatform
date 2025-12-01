using FileSharingPlatform.Data.UnitOfWork;
using FileSharingPlatform.DTOs.ShareLinkDTOs;
using FileSharingPlatform.Helpers;
using FileSharingPlatform.Models;

namespace FileSharingPlatform.Services
{
    public class ShareLinkService : IShareLinkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShareLinkService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ShareLinkeResponseDto> CreateFileShareLinkAsync(
           Guid userId,
           Guid fileId,
           CreateShareLinkDto dto)
        {
            // Validate file exists and user owns it
            var file = await _unitOfWork.Files.GetByIdAsync(fileId);

            if (file == null)
            {
                throw new InvalidOperationException("File not found");
            }

            if (file.UserId != userId)
            {
                throw new UnauthorizedAccessException("You don't own this file");
            }

            // Disable existing active link
            var existingLink = await _unitOfWork.ShareLinks.GetActiveFileShareLinkAsync(fileId);
            if (existingLink != null)
            {
                existingLink.IsActive = false;
                _unitOfWork.ShareLinks.Update(existingLink);
            }

            // Create new share link
            var shareLink = new PublicShareLink
            {
                ShareLinkId = Guid.NewGuid(),
                Token = ShareLinkHelper.GenerateSecureToken(32),
                FileId = fileId,
                UserId = userId,
                PasswordHash = dto.Password != null ? BCrypt.Net.BCrypt.HashPassword(dto.Password) : null,
                ExpiresAt = DateTime.UtcNow.AddDays(dto.ExpirationDays),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ShareLinks.AddAsync(shareLink);
            await _unitOfWork.SaveChangesAsync();

            var baseUrl = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}";

            return new ShareLinkeResponseDto
            {
                ShareLinkId = shareLink.ShareLinkId,
                ShareUrl = $"{baseUrl}/api/public/share/{shareLink.Token}",
                ExpiresAt = shareLink.ExpiresAt,
            };
        }



        public async Task<ShareLinkeResponseDto> CreateFolderShareLinkAsync(
            Guid userId,
            Guid folderId,
            CreateShareLinkDto dto)
        {
            // Validate folder exists and user owns it
            var folder = await _unitOfWork.Folders.GetByIdAsync(folderId);

            if (folder == null)
            {
                throw new InvalidOperationException("Folder not found");
            }

            if (folder.UserId != userId)
            {
                throw new UnauthorizedAccessException("You don't own this folder");
            }

            // Disable existing active link
            var existingLink = await _unitOfWork.ShareLinks.GetActiveFolderShareLinkAsync(folderId);
            if (existingLink != null)
            {
                existingLink.IsActive = false;
                _unitOfWork.ShareLinks.Update(existingLink);
            }

            // Create new share link
            var shareLink = new PublicShareLink
            {
                ShareLinkId = Guid.NewGuid(),
                Token = ShareLinkHelper.GenerateSecureToken(32),
                FolderId = folderId,
                UserId = userId,
                PasswordHash = dto.Password != null ? BCrypt.Net.BCrypt.HashPassword(dto.Password) : null,
                ExpiresAt = DateTime.UtcNow.AddDays(dto.ExpirationDays),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ShareLinks.AddAsync(shareLink);
            await _unitOfWork.SaveChangesAsync();

            var baseUrl = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}";

            return new ShareLinkeResponseDto
            {
                ShareLinkId = shareLink.ShareLinkId,
                ShareUrl = $"{baseUrl}/api/public/share/{shareLink.Token}",
                ExpiresAt = shareLink.ExpiresAt,
            };
        }



        public async Task<ShareLinkDetailsDto> GetPublicShareDetailsAsync(string token, string? password)
        {
            var shareLink = await _unitOfWork.ShareLinks.GetByTokenAsync(token);

            if (shareLink == null || !shareLink.IsActive || shareLink.ExpiresAt < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Link is invalid or expired");
            }

            // Verify password if protected
            if (shareLink.PasswordHash != null)
            {
                if (password == null || !BCrypt.Net.BCrypt.Verify(password, shareLink.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Invalid password");
                }
            }

            // Update last accessed
            shareLink.LastAccessedAt = DateTime.UtcNow;
            _unitOfWork.ShareLinks.Update(shareLink);

            await _unitOfWork.SaveChangesAsync();

            // Return details based on type
            if (shareLink.FileId.HasValue && shareLink.File != null)
            {
                return new ShareLinkDetailsDto
                {
                    Type = "file",
                    Name = shareLink.File.Name,
                    Size = shareLink.File.Size,
                    MimeType = shareLink.File.MimeType,
                    CreatedAt = shareLink.CreatedAt,
                    RequiresPassword = shareLink.PasswordHash != null
                };
            }
            else if (shareLink.FolderId.HasValue && shareLink.Folder != null)
            {
                return new ShareLinkDetailsDto
                {
                    Type = "folder",
                    Name = shareLink.Folder.Name,
                    Size = null,
                    MimeType = null,
                    CreatedAt = shareLink.CreatedAt,
                    RequiresPassword = shareLink.PasswordHash != null
                };
            }

            throw new InvalidOperationException("Invalid share link");
        }


        public async Task<byte[]> DownloadPublicFileAsync(string token, string? password)
        {
            var shareLink = await _unitOfWork.ShareLinks.GetByTokenAsync(token);

            if (shareLink == null || !shareLink.IsActive || shareLink.ExpiresAt < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Link is invalid or expired");
            }

            if (shareLink.FileId == null || shareLink.File == null)
            {
                throw new InvalidOperationException("This link is not for a file");
            }

            // Verify password if protected
            if (shareLink.PasswordHash != null)
            {
                if (password == null || !BCrypt.Net.BCrypt.Verify(password, shareLink.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Invalid password");
                }
            }

            if (!System.IO.File.Exists(shareLink.File.StoragePath))
            {
                throw new InvalidOperationException("File not found on disk");
            }

            return await System.IO.File.ReadAllBytesAsync(shareLink.File.StoragePath);
        }

        public async Task DisableShareLinkAsync(Guid userId, Guid shareLinkId)
        {
            var shareLink = await _unitOfWork.ShareLinks.GetByIdAsync(shareLinkId);

            if (shareLink == null)
            {
                throw new InvalidOperationException("Share link not found");
            }

            if (shareLink.UserId != userId)
            {
                throw new UnauthorizedAccessException("You don't own this share link");
            }

            shareLink.IsActive = false;
            _unitOfWork.ShareLinks.Update(shareLink);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
