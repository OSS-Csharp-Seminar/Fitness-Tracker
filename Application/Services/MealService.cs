using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _mealRepository;

        public MealService(IMealRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<Meal> CreateMeal(Meal meal)
        {
            if (meal == null)
                throw new ArgumentNullException(nameof(meal));

            meal.Id = Guid.NewGuid();
            meal.CreatedAt = DateTime.UtcNow;

            return await _mealRepository.AddAsync(meal);
        }

        public async Task<List<Meal>> GetByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            var query = await _mealRepository.GetByUserIdAsync(userId);
            return await query.ToListAsync();
        }

        public async Task<List<Meal>> GetByDate(Guid userId, DateOnly date)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            if (date == default)
                throw new ArgumentException("Date is required");

            var query = await _mealRepository.GetByDateAsync(userId, date);
            return await query.ToListAsync();
        }

        public async Task<int> GetTotalCaloriesForDate(Guid userId, DateOnly date)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            return await _mealRepository.GetTotalCaloriesForDateAsync(userId, date);
        }

        public async Task<bool> UpdateMeal(Meal meal)
        {
            if (meal == null)
                throw new ArgumentNullException(nameof(meal));

            if (meal.Id == Guid.Empty)
                throw new ArgumentException("Meal ID is required");

            return await _mealRepository.UpdateAsync(meal);
        }

        public async Task<bool> DeleteMeal(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Meal ID is required");

            return await _mealRepository.DeleteAsync(id);
        }
    }
}
