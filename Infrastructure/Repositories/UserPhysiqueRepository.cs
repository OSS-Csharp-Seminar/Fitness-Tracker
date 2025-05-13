using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserPhysiqueRepository : IUserPhysiqueRepository
    {
        private readonly ApplicationDbContext _context;

        public UserPhysiqueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserPhysique> AddAsync(UserPhysique userPhysique)
        {
            await _context.AddAsync(userPhysique);
            await _context.SaveChangesAsync();

            return userPhysique;    
        }
        public async Task<UserPhysique> GetById(Guid id)
        {
            return await _context.UserPhysiques
                .Include(p => p.User) 
                .FirstOrDefaultAsync(up => up.Id == id);
        }

        public async Task<IEnumerable<UserPhysique>> GetByUerId(Guid id)
        {
            return await _context.UserPhysiques
                .Where(up => up.UserId == id)
                .OrderByDescending(up => up.Id) 
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(UserPhysique userPhysique)
        {
            try
            {
                _context.UserPhysiques.Update(userPhysique);
                int affectiveRiws = await _context.SaveChangesAsync();

                return affectiveRiws > 0;     
            }
            catch (Exception ex) { 
                return false;
            }

        }
    }
}
