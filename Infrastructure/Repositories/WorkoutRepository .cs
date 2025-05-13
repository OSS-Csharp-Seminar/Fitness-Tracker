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
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkoutRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Workout> GetByIdAsync(Guid id)
        {
            return await _context.Workouts
                .FirstOrDefaultAsync(x =>x.Id == id);
        }

        public async Task<IEnumerable<Workout>> AllAsync()
        {
            _context.Workouts
                .Include(x => x.Id)
                .ToListAsync();

            return _context.Workouts;
        }

        public async Task<IEnumerable<Workout>> GetByTagAsync(string tag)
        {
            var workouts = await _context.Workouts
                 .Where(w => w.tag.ToString() == tag)
                 .ToListAsync();

            return workouts;
        }
    }
}
