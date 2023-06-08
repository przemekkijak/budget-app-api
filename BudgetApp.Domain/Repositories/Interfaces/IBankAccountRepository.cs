using BudgetApp.Domain.Entities;

namespace BudgetApp.Domain.Repositories.Interfaces;

public interface IBankAccountRepository : IBaseRepository<BankAccountEntity>
{
    Task<List<BankAccountEntity>> GetForBudget(int budgetId);
    Task UpdateAccountAmount(int bankAccountId, decimal amount);
}