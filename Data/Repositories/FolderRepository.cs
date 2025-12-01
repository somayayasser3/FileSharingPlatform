using FileSharingPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSharingPlatform.Data.Repositories
{
    public class FolderRepository : Repository<Folder>, IFolderRepository
    {
        public FolderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> FolderNameExistsAsync(string name, Guid? parentFolderId, Guid userId)
        {
            return await dbSet.AnyAsync(f =>
               f.Name == name &&
               f.ParentFolderId == parentFolderId &&
               f.UserId == userId);
        }

        public async Task<IEnumerable<Folder>> GetUserFoldersAsync(Guid userId)
        {
            return await dbSet
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<Folder?> GetWithFilesAsync(Guid folderId)
        {
            return await dbSet
                .Include(f => f.Files)
                .Include(f => f.SubFolders)
                .FirstOrDefaultAsync(f => f.FolderId == folderId);
        }
    }
}
