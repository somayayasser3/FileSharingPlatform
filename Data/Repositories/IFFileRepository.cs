using FileSharingPlatform.Models;

namespace FileSharingPlatform.Data.Repositories
{
    public interface IFFileRepository : IRepository<FFile>
    {
        Task<IEnumerable<FFile>> GetFilesByFolderAsync(Guid folderId);
        Task<bool> FileNameExistsAsync(string name, Guid folderId);
        Task<long> GetUserStorageUsedAsync(Guid userId);
    }
}
