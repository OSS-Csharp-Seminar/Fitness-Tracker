using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> AddAsync(User user, string password);
        Task<bool> GetByEmailAsync(string email);
        Task<bool> GetByUsernameAsync(string username);
        Task<User> GetEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}
