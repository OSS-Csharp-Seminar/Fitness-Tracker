using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkoutPlanRepository : BaseRepository<WorkoutPlan>, IWorkoutPlanRepository
    {
        public WorkoutPlanRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<WorkoutPlan>> GetByUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var query = _context.Set<WorkoutPlan>()
                .Where(wp => wp.UserId == user.Id)
                .Include(wp => wp.User)
                .Include(wp => wp.Workouts)
                .OrderByDescending(wp => wp.StartDate);

            return query;
        }

        public async Task<bool> HasActiveWorkoutPlanAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var activePlan = await _context.WorkoutPlans
                .Where(wp => wp.UserId == user.Id)
                .FirstOrDefaultAsync();

            return activePlan != null;
        } 
      

        public override async Task<WorkoutPlan> GetByIdAsync(Guid id)
        {
            return await _context.Set<WorkoutPlan>()
                .Include(wp => wp.User)
                .Include(wp => wp.Workouts)
                    .ThenInclude(w => w.WorkoutCatalog)
                .FirstOrDefaultAsync(wp => wp.Id == id);
        }

        public override async Task<IEnumerable<WorkoutPlan>> GetAllAsync()
        {
            return await _context.Set<WorkoutPlan>()
                .Include(wp => wp.User)
                .Include(wp => wp.Workouts)
                .OrderByDescending(wp => wp.StartDate)
                .ToListAsync();
        }
    }
}