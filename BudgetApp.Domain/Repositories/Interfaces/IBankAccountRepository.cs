using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;

namespace BudgetApp.Domain.Interfaces.Repositories;

public interface IBankAccountRepository : IBaseRepository<BankAccountEntity>
{
    Task<List<BankAccountEntity>> GetForBudget(int budgetId);
    Task UpdateAccountAmount(int bankAccountId, decimal amount);
}