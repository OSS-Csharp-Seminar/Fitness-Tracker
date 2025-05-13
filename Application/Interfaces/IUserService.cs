
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user, string password);
        Task<(string Email, string UserName)> GetUserName_EmailAsync(Guid userId); 
        Task Check_Email_UsernameAsync(string username, string email);
        Task<(string userId, string username)> Login(string email, string password);
    }
}