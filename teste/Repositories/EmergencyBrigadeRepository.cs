using FireBrigade.Data;
using FireBrigade.Domain.Entities;
using FireBrigade.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FireBrigade.Infra.Data.Repositories
{
    public class EmergencyBrigadeRepository : IEmergencyBrigadeRepository
    {
        private readonly FireBrigadeContext _fireBrigadeContext;

        public EmergencyBrigadeRepository(FireBrigadeContext fireBrigadeContext)
        {
            _fireBrigadeContext = fireBrigadeContext;
        }
        public IEnumerable<EmergencyBrigade> GetAll()
        {
            return _fireBrigadeContext.EmergencyBrigades;
        }

        public async Task<EmergencyBrigade> GetById(Guid id)
        {
            return await _fireBrigadeContext.EmergencyBrigades.FindAsync(id);
        }

        public async Task Create(EmergencyBrigade emergencyBrigade)
        {
            await _fireBrigadeContext.EmergencyBrigades.AddAsync(emergencyBrigade);
            await _fireBrigadeContext.SaveChangesAsync();
        }

        public async Task<EmergencyBrigade> Update(EmergencyBrigade emergencyBrigade)
        {
            _fireBrigadeContext.EmergencyBrigades.Update(emergencyBrigade);
            await _fireBrigadeContext.SaveChangesAsync();
            return emergencyBrigade;
        }

        public Task Delete(EmergencyBrigade emergencyBrigade)
        {
            _fireBrigadeContext.EmergencyBrigades.Remove(emergencyBrigade);
            return _fireBrigadeContext.SaveChangesAsync();
        }
    }
}
