
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<User> AddAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Nije moguće stvoriti korisnika: {errors}");
            }
            if (user.Role != default)
            {
                await _userManager.AddToRoleAsync(user, user.Role.ToString());
            }
            return user;
        }
        public async Task<User> GetByIdAsync(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }
        public async Task<bool> GetByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                return true;
            return false;
        }
        public async Task<bool> GetByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null) return true;
            return false;
        }
        public async Task<User> GetEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            if (user == null)
                throw new ArgumentException($"Korisnik sa emailom ne postoji!");
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}