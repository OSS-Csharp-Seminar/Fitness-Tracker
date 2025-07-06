using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(UserManager<User> userManager, ApplicationDbContext context)
            : base(context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<User> AddAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error creating: {errors}");
            }

            if (user.Role != default)
            {
                await _userManager.AddToRoleAsync(user, user.Role.ToString());
            }

            return user;
        }

        public override async Task<User> GetByIdAsync(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public override async Task<bool> UpdateAsync(User user)
        {
            try
            {
                // custom svojstva
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
                if (existingUser == null)
                    return false;

                // Update custom 
                existingUser.Birthday = user.Birthday;
                existingUser.Gender = user.Gender;
                existingUser.Height = user.Height;
                existingUser.Allergies = user.Allergies;
                existingUser.Diagnosis = user.Diagnosis;
                existingUser.Role = user.Role;

                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.NormalizedUserName = user.UserName.ToUpper();
                existingUser.NormalizedEmail = user.Email.ToUpper();

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update error: {ex}");
                return false;
            }
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null) return false;

                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> GetByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<bool> GetByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user != null;
        }

        public async Task<User> GetEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            if (user == null)
                throw new ArgumentException("User with that email does not exist!");

            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}