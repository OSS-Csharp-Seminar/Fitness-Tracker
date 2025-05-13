// Domain/Interfaces/IUserRepository.cs
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user, string password);
        Task<User> GetByIdAsync(Guid userId);
        Task<bool> GetByEmailAsync(string email);
        Task<bool> GetByUsernameAsync(string username);
        Task<User> GetEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}