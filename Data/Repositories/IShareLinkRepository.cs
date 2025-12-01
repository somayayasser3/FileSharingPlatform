using FileSharingPlatform.Models;

namespace FileSharingPlatform.Data.Repositories
{
    public interface IShareLinkRepository : IRepository<PublicShareLink>
    {
        Task<PublicShareLink?> GetByTokenAsync(string token);
        Task<PublicShareLink?> GetActiveFileShareLinkAsync(Guid fileId);
        Task<PublicShareLink?> GetActiveFolderShareLinkAsync(Guid folderId);
    }
}
