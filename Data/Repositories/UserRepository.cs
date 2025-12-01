using FileSharingPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSharingPlatform.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await dbSet.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await dbSet.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
