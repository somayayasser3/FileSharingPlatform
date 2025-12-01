using FileSharingPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSharingPlatform.Data.Repositories
{
    public class ShareLinkRepository : Repository<PublicShareLink>, IShareLinkRepository
    {
        public ShareLinkRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PublicShareLink?> GetActiveFileShareLinkAsync(Guid fileId)
        {
            return await dbSet.FirstOrDefaultAsync(s =>
                s.FileId == fileId &&
                s.IsActive &&
                s.ExpiresAt > DateTime.UtcNow);
        }

        public async Task<PublicShareLink?> GetActiveFolderShareLinkAsync(Guid folderId)
        {
            return await dbSet.FirstOrDefaultAsync(s =>
                 s.FolderId == folderId &&
                 s.IsActive &&
                 s.ExpiresAt > DateTime.UtcNow);
        }

        public async Task<PublicShareLink?> GetByTokenAsync(string token)
        {
            return await dbSet
                .Include(s => s.File)
                .Include(s => s.Folder)
                .FirstOrDefaultAsync(s => s.Token == token);
        }
    }
}
