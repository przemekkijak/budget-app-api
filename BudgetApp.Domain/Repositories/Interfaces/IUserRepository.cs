using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;

namespace BudgetApp.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<UserEntity?> GetByEmail(string email);
}