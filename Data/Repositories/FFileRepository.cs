using FileSharingPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSharingPlatform.Data.Repositories
{
    public class FFileRepository : Repository<FFile>, IFFileRepository
    {
        public FFileRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> FileNameExistsAsync(string name, Guid folderId)
        {
            return await dbSet.AnyAsync(f => f.Name == name && f.FolderId == folderId);
        }

        public async Task<IEnumerable<FFile>> GetFilesByFolderAsync(Guid folderId)
        {
            return await dbSet
                .Where(f => f.FolderId == folderId)
                .ToListAsync();
        }

        public async Task<long> GetUserStorageUsedAsync(Guid userId)
        {
            return await dbSet
                .Where(f => f.UserId == userId)
                .SumAsync(f => (long?)f.Size) ?? 0;
        }
    }
}
