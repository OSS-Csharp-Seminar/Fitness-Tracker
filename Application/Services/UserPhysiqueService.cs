using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Application.Interfaces;
using Domain.Enum;

namespace Application.Services
{
    public class UserPhysiqueService : IUserPhysiqueService
    {
        private readonly IUserPhysiqueRepository _userPhysiqueRepository;

        public UserPhysiqueService(IUserPhysiqueRepository userPhysiqueRepository)
        {
            _userPhysiqueRepository = userPhysiqueRepository;
        }

        public async Task<UserPhysique> userPhysique(UserPhysique userPhysique)
        {
            if (userPhysique == null)
                throw new ArgumentNullException(nameof(userPhysique));

            if (userPhysique.Id == Guid.Empty)
                userPhysique.Id = Guid.NewGuid();

            return await _userPhysiqueRepository.AddAsync(userPhysique);
        }
        public async Task<bool> UpdateUserPysique(UserPhysique updateDto)
        {
            try
            {
                var existingPhysique = await _userPhysiqueRepository.GetById(updateDto.Id);
                if (existingPhysique == null)
                    return false;

                if (updateDto.Weight != 0)
                    existingPhysique.Weight = updateDto.Weight;

                if (updateDto.Height != 0)
                    existingPhysique.Height = updateDto.Height;

                if (updateDto.dietType != default(DietType))
                    existingPhysique.dietType = updateDto.dietType;

                if (updateDto.Allergies != null && updateDto.Allergies.Any())
                {
                    if (existingPhysique.Allergies == null)
                        existingPhysique.Allergies = new List<string>();

                    foreach (var allergy in updateDto.Allergies)
                    {
                        if (!existingPhysique.Allergies.Contains(allergy))
                            existingPhysique.Allergies.Add(allergy);
                    }
                }

                if (updateDto.Diagnosis != null && updateDto.Diagnosis.Any())
                {
                    if (existingPhysique.Diagnosis == null)
                        existingPhysique.Diagnosis = new List<string>();

                    foreach (var diagnosis in updateDto.Diagnosis)
                    {
                        if (!existingPhysique.Diagnosis.Contains(diagnosis))
                            existingPhysique.Diagnosis.Add(diagnosis);
                    }
                }

                existingPhysique.Date = DateOnly.FromDateTime(DateTime.Now);

                return await _userPhysiqueRepository.UpdateAsync(existingPhysique);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
    
}