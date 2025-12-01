using FileSharingPlatform.DTOs.ShareLinkDTOs;

namespace FileSharingPlatform.Services
{
    public interface IShareLinkService
    {
        Task<ShareLinkeResponseDto> CreateFileShareLinkAsync(Guid userId, Guid fileId, CreateShareLinkDto dto);
        Task<ShareLinkeResponseDto> CreateFolderShareLinkAsync(Guid userId, Guid folderId, CreateShareLinkDto dto);
        Task<ShareLinkDetailsDto> GetPublicShareDetailsAsync(string token, string? password);
        Task<byte[]> DownloadPublicFileAsync(string token, string? password);
        Task DisableShareLinkAsync(Guid userId, Guid shareLinkId);
    }
}
