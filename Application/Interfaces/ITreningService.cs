using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Interfaces
{
    public interface ITreningService 
    {
        Task<Trening> CreateTrening(Trening trening);
        Task<IEnumerable<Trening>> GetAllTrenings(Guid WorkoutPlanId);
    }
}
