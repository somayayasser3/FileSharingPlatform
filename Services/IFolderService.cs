using FileSharingPlatform.DTOs.FolderDTOs;
using FileSharingPlatform.DTOs.FoldersAndFilesListingDTOs;

namespace FileSharingPlatform.Services
{
    public interface IFolderService
    {
        Task<FolderResponseDto> CreateFolderAsync(Guid userId, CreateFolderDto createFolderDto);
        Task<FolderItemsResponseDto> GetFolderItemsAsync(Guid userId,Guid folderId,int page,int pageSize,string sortBy,string order);
    }
}
