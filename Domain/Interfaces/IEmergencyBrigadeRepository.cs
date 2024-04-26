using FireBrigade.Domain.Entities;

namespace FireBrigade.Domain.Interfaces;

public interface IEmergencyBrigadeRepository
{
    Task<EmergencyBrigade> GetById(Guid id);
    IEnumerable<EmergencyBrigade> GetAll();
    Task Create(EmergencyBrigade emergencyBrigade);
    Task<EmergencyBrigade> Update(EmergencyBrigade emergencyBrigade);
    Task Delete(EmergencyBrigade emergencyBrigade);
}
