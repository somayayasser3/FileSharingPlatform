using FileSharingPlatform.Data.Repositories;

namespace FileSharingPlatform.Data.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepository Users { get; }
        IFolderRepository Folders { get; }
        IFFileRepository Files { get; }
        IShareLinkRepository ShareLinks { get; }

        Task<int> SaveChangesAsync();
    }
}
