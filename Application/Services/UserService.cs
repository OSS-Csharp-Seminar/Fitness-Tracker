// Application/Services/UserService.cs
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Data;
using System.Threading.Tasks;
namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task Check_Email_UsernameAsync(string username, string email)
        {
            if (await _userRepository.GetByEmailAsync(email))
                throw new ArgumentException($"User sa {email} vecpostoji!");
            if (await _userRepository.GetByUsernameAsync(username))
                throw new ArgumentException($"Korisnik sa {username} vec postoji!");
        }
        public async Task<User> CreateUserAsync(User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Lozinka je obavezna");
            // Postavljanje osnovnih vrijednosti ako je potrebno
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();
            await Check_Email_UsernameAsync(user.UserName, user.Email);
            // Pozivanje repository metode
            return await _userRepository.AddAsync(user, password);
        }
        public async Task<(string Email, string UserName)> GetUserName_EmailAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId), "ID korisnika ne moze biti prazan!");
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"Korisnik sa Id-om: {userId} ne postoji!");
            return (user.Email, user.UserName);
        }
        public async Task<(string userId, string username)> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email je obavezan!");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password je obavezan");

            var user = await _userRepository.GetEmailAsync(email);

            if (user == null)
                throw new ArgumentException($"{email} is not registered.");

            bool isPasswordValid = await _userRepository.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
                throw new ArgumentException("Wrong password");
            return (user.Email, user.UserName);
        }
    }
}