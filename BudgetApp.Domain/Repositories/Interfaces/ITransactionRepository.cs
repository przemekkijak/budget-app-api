using BudgetApp.Domain.Entities;

namespace BudgetApp.Domain.Repositories.Interfaces;

public interface ITransactionRepository : IBaseRepository<TransactionEntity>
{
    Task<List<TransactionEntity>> GetForBudget(int budgetId, DateTime? startDate, DateTime? endDate);
    Task<HashSet<string>> GetHashListForBudget(int budgetId);
}