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
    public class TrningRepository : ITreningRepository
    {
        private readonly ApplicationDbContext _context;

        public TrningRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Trening> AddAsync(Trening trening, WorkoutPlan workoutPlan)
        {
            trening.WorkoutPlanId = workoutPlan.Id;
            trening.UserId = workoutPlan.UserId;
            trening.Date = DateTime.UtcNow;
          
            await _context.Trenings.AddAsync(trening);
            await _context.SaveChangesAsync();

            return trening;
        }

        public async Task<Trening> GetByIdAsync(Guid id)
        {
            return await _context.Trenings
                .Include(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Trening>> GetAllAsync(Guid WorkoutPlanId)
        {
            _context.Trenings
                .Include (x => x.WorkoutPlanId)
                .ToListAsync();
            return _context.Trenings;

        }
    }
}
