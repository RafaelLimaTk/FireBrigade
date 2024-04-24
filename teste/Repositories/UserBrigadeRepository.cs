using FireBrigade.Data;
using FireBrigade.Domain.Entities;
using FireBrigade.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FireBrigade.Infra.Data.Repositories;

public class UserBrigadeRepository : IUserBrigadeRepository
{
    private readonly FireBrigadeContext _fireBrigadeContext;

    public UserBrigadeRepository(FireBrigadeContext fireBrigadeContext)
        => _fireBrigadeContext = fireBrigadeContext;

    public async Task<UserBrigade> GetByUser(string email, string password)
        => await _fireBrigadeContext.Users
            .FirstOrDefaultAsync(user => user.Email == email.ToLower() && user.Password == password.ToLower())
            ?? throw new Exception("Usuário não encontrado.");

    public async Task Create(UserBrigade userBrigade)
    {
        await _fireBrigadeContext.Users.AddAsync(userBrigade);
        await _fireBrigadeContext.SaveChangesAsync();
    }

    public async Task<UserBrigade> Update(UserBrigade userBrigade)
    {
        _fireBrigadeContext.Users.Update(userBrigade);
        await _fireBrigadeContext.SaveChangesAsync();
        return userBrigade;
    }

    public async Task Delete(UserBrigade userBrigade)
    {
        _fireBrigadeContext.Users.Remove(userBrigade);
        await _fireBrigadeContext.SaveChangesAsync();
    }
}
