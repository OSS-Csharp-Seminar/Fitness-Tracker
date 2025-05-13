using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWorkoutRepository
    {
        Task<Workout> GetByIdAsync(Guid id);
        Task<IEnumerable<Workout>> AllAsync();
        Task<IEnumerable<Workout>> GetByTagAsync(string tag);


    }
}
