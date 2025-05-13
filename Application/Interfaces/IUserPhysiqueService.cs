using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Interfaces
{
    public interface IUserPhysiqueService
    {
        Task<UserPhysique> userPhysique(UserPhysique userPhysique);
        Task<bool> UpdateUserPysique(UserPhysique updateDto);
    }
}
