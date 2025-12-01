using FileSharingPlatform.DTOs.FileDTOs;

namespace FileSharingPlatform.Services
{
    public interface IFileService
    {
        Task<FileResponseDto> UploadFileAsync(Guid userId, UploadFileDto uploadFileDto);
        Task<(byte[] fileContent, string fileName, string mimeType)> DownloadFileAsync(Guid userId, Guid fileId);
    }
}
