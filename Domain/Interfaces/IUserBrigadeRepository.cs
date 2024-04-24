using FireBrigade.Domain.Entities;

namespace FireBrigade.Domain.Interfaces;

public interface IUserBrigadeRepository
{
    Task<UserBrigade> GetByUser(string email, string password);
    Task Create(UserBrigade userBrigade);
    Task<UserBrigade> Update(UserBrigade userBrigade);
    Task Delete(UserBrigade userBrigade);
}
