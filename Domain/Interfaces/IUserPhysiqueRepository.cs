using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserPhysiqueRepository
    {
        Task<UserPhysique> AddAsync(UserPhysique userPhysique);
        Task<UserPhysique> GetById(Guid id);
        Task<IEnumerable<UserPhysique>> GetByUerId(Guid id);
        Task<bool> UpdateAsync(UserPhysique userPhysique);

    }
}
