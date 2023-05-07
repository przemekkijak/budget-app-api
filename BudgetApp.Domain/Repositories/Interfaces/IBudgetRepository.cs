
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;

namespace BudgetApp.Domain.Interfaces.Repositories;

public interface IBudgetRepository : IBaseRepository<BudgetEntity>
{
    Task<BudgetEntity?> GetDefault(int userId);
    Task<BudgetEntity?> GetByName(int userId, string name);
}