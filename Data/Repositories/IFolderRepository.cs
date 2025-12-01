using FileSharingPlatform.Models;

namespace FileSharingPlatform.Data.Repositories
{
    public interface IFolderRepository : IRepository<Folder>
    {
        Task<Folder?> GetWithFilesAsync(Guid folderId);
        Task<IEnumerable<Folder>> GetUserFoldersAsync(Guid userId);
        Task<bool> FolderNameExistsAsync(string name, Guid? parentFolderId, Guid userId);
    }
}
