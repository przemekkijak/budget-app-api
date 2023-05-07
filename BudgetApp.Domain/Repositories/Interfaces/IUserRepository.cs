using BudgetApp.Domain.Entities;

namespace BudgetApp.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<UserEntity?> GetByEmail(string email);
}