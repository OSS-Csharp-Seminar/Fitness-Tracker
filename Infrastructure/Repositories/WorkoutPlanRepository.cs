using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class WorkoutPlanRepository : IWorkoutPlanRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkoutPlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WorkoutPlan> AddAsync(WorkoutPlan workoutPlan)
        {
            await _context.WorkoutPlans.AddAsync(workoutPlan);
            await _context.SaveChangesAsync();

            return workoutPlan;
        }

        public async Task<WorkoutPlan> GetByIdAsync(Guid id)
        {
            return await _context.WorkoutPlans
                .Include(x => x.Id)
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<WorkoutPlan> GetByUserIdAsync(Guid userId)
        {
            return await _context.WorkoutPlans
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.UserId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(WorkoutPlan workoutPlan)
        {
            try
            {
                _context.WorkoutPlans.Update(workoutPlan);
                int affectiveRows = await _context.SaveChangesAsync();

                return affectiveRows > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<WorkoutPlan>> GetAllAsync(Guid userId)
        {
            return await _context.WorkoutPlans
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id) 
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var workoutPlan = await _context.WorkoutPlans.FindAsync(id);
            if (workoutPlan != null)
                throw new ArgumentException("The workout do not exists!");

            try
            {
                _context.WorkoutPlans.Remove(workoutPlan);
                int affectiveRows = await _context.SaveChangesAsync();

                return affectiveRows > 0;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
