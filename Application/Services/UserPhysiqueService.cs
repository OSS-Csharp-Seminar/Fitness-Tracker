using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore; 

namespace Application.Services
{
    public class UserPhysiqueService : IUserPhysiqueService
    {
        private readonly IUserPhysiqueRepository _userPhysiqueRepository;

        public UserPhysiqueService(IUserPhysiqueRepository userPhysiqueRepository)
        {
            _userPhysiqueRepository = userPhysiqueRepository;
        }

        public async Task<UserPhysique> CreateUserPhysique(UserPhysique userPhysique)
        {
            if (userPhysique == null)
                throw new ArgumentNullException(nameof(userPhysique));

            if (userPhysique.UserId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            if (userPhysique.Weight <= 0)
                throw new ArgumentException("Weight must be greater than 0");

            if (userPhysique.Date == default)
                userPhysique.Date = DateOnly.FromDateTime(DateTime.Now);

            var createdPhysique = await _userPhysiqueRepository.AddAsync(userPhysique);

            return createdPhysique;
        }

        public async Task<decimal> GetWeightProgress(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            return await _userPhysiqueRepository.GetWeightProgressAsync(userId);
        }

        public async Task<List<UserPhysique>> GetAllPhysiques()
        {
            var physiques = await _userPhysiqueRepository.GetAllAsync();
            return physiques.ToList();
        }

        public async Task<List<UserPhysique>> GetByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            var query = await _userPhysiqueRepository.GetByUserId(userId);
            return await query.ToListAsync(); // Sada radi!
        }

        public async Task<UserPhysique> GetLatestPhysique(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            var query = await _userPhysiqueRepository.GetByUserId(userId);
            return await query.FirstOrDefaultAsync(); // Sada radi!
        }

        public async Task<bool> UpdatePhysique(UserPhysique userPhysique)
        {
            if (userPhysique == null)
                throw new ArgumentNullException(nameof(userPhysique));

            if (userPhysique.Id == Guid.Empty)
                throw new ArgumentException("Physique ID is required");

            if (userPhysique.Weight <= 0)
                throw new ArgumentException("Weight must be greater than 0");

            return await _userPhysiqueRepository.UpdateAsync(userPhysique);
        }

        public async Task<bool> DeletePhysique(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Physique ID is required");

            return await _userPhysiqueRepository.DeleteAsync(id);
        }

        public async Task<UserPhysique> AddWeightMeasurement(User user, float weight, int? bmi = null)
        {
            var physique = new UserPhysique
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Weight = weight,
                BMI = bmi ?? CalculateBMI(weight, user),
                Date = DateOnly.FromDateTime(DateTime.Now)
            };

            return await CreateUserPhysique(physique);
        }

        public async Task<string> GetProgressSummary(Guid userId)
        {
            try
            {
                var progress = await GetWeightProgress(userId);

                if (progress == 0)
                    return "No weight change recorded or insufficient data.";

                if (progress > 0)
                    return $"Weight increased by {progress:F1}kg since last measurement.";

                return $"Weight decreased by {Math.Abs(progress):F1}kg since last measurement.";
            }
            catch (Exception)
            {
                return "Unable to calculate weight progress.";
            }
        }

        public int CalculateBMI(float weight, User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (weight <= 0)
                throw new ArgumentException("Weight must be greater than 0");

            if (user.Height <= 0)
                throw new ArgumentException("User height must be greater than 0");

            // BMI formula: weight (kg) / height (m)²
            float heightInMeters = user.Height / 100f; 
            float bmi = weight / (heightInMeters * heightInMeters);

            return (int)Math.Round(bmi);
        }
    }
}
