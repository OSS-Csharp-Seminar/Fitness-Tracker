using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enum;

namespace Domain.Interfaces
{
    public interface IWorkoutRepository : IBaseRepository<Workout>
    {
        Task<IQueryable<Workout>> GetByUserIdAsync(Guid userId);
        Task<IQueryable<Workout>> GetByPlanIdAsync(Guid planId);
        Task<IQueryable<Workout>> GetByDateAsync(DateOnly date);
        Task<IQueryable<Workout>> GetTodayWorkoutsAsync(User user);
        Task<IQueryable<Workout>> GetCompletedWorkoutsAsync(User user);
        Task<int> GetCompletedCountAsync(User user, DateOnly date);
    }
}
