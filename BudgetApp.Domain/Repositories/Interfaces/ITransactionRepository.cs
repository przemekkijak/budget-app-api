using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;

namespace BudgetApp.Domain.Interfaces.Repositories;

public interface ITransactionRepository : IBaseRepository<TransactionEntity>
{
    Task<List<TransactionEntity>> GetForBudget(int budgetId, DateTime? startDate, DateTime? endDate);
}