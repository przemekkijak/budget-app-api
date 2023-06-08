using BudgetApp.Domain.Entities;

namespace BudgetApp.Domain.Repositories.Interfaces;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<UserEntity?> GetByEmail(string email);
}