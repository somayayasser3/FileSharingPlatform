using FileSharingPlatform.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FileSharingPlatform.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbcontext;

        public IUserRepository Users { get; private set; }
        public IFolderRepository Folders { get; private set; }
        public IFFileRepository Files { get; private set; }
        public IShareLinkRepository ShareLinks { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            dbcontext = context;
            Users = new UserRepository(dbcontext);
            Folders = new FolderRepository(dbcontext);
            Files = new FFileRepository(dbcontext);
            ShareLinks = new ShareLinkRepository(dbcontext);
        }

        public void Dispose()
        {
            dbcontext.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbcontext.SaveChangesAsync();
        }
    }
}
