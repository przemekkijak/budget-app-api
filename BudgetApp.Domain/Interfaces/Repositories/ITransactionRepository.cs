using BudgetApp.Domain.Entities;

namespace BudgetApp.Domain.Interfaces.Repositories;

public interface ITransactionRepository : IBaseRepository<TransactionEntity>
{
    Task<List<TransactionEntity>> GetForBudget(int budgetId);
}