using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CheckEmailUsernameAsync(string username, string email)
        {
            if (await _userRepository.GetByEmailAsync(email))
                throw new ArgumentException($"User with email {email} already exists!");

            if (await _userRepository.GetByUsernameAsync(username))
                throw new ArgumentException($"User with username {username} already exists!");
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password is required");

            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();

            await CheckEmailUsernameAsync(user.UserName, user.Email);

            return await _userRepository.AddAsync(user, password);
        }

        public async Task<(string UserId, string Username)> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email is required!");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password is required!");

            var user = await _userRepository.GetEmailAsync(email);
            if (user == null)
                throw new ArgumentException($"{email} is not registered.");

            bool isPasswordValid = await _userRepository.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
                throw new ArgumentException("Wrong password");

            return (user.Id.ToString(), user.UserName);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id), "User ID cannot be empty!");

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID: {id} does not exist!");

            return user;
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.Id == Guid.Empty)
                throw new ArgumentException("User ID is required");

            try
            {
                return await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to update user: {ex.Message}", ex);
            }
        }

        public int CalculateAge(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.Birthday == default)
                return 30; // Default age

            var today = DateOnly.FromDateTime(DateTime.Now);
            var age = today.Year - user.Birthday.Year;

            // Provjeri da li je rođendan već prošao ove godine
            if (today.DayOfYear < user.Birthday.DayOfYear)
                age--;

            return Math.Max(16, Math.Min(100, age)); // Ograniči na razumne vrijednosti
        }

        public float GetHeight(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return user.Height;
        }
    }
}