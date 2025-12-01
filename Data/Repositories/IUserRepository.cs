using FileSharingPlatform.Models;

namespace FileSharingPlatform.Data.Repositories
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
    }
}
