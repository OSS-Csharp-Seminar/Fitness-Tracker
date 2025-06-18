
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task CheckEmailUsernameAsync(string username, string email);
        Task<User> CreateUserAsync(User user, string password);
        Task<(string UserId, string Username)> Login(string email, string password); // ISPRAVKA
        Task<User> GetByIdAsync(Guid id);

    }
}