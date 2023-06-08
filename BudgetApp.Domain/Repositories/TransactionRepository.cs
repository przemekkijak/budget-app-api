using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;
using Dommel;

namespace BudgetApp.Domain.Repositories;

public class TransactionRepository : BaseRepository<TransactionEntity>, ITransactionRepository
{
    public TransactionRepository(AppSettings appSettings) : base(appSettings)
    {
    }

    public async Task<List<TransactionEntity>> GetForBudget(int budgetId, DateTime? startDate = null, DateTime? endDate = null)
    {
        using var con = CreateConnection();
        return (await con
            .SelectAsync<TransactionEntity>(a => a.BudgetId == budgetId && a.IsDeleted == false && a.CreateDate >= startDate && a.CreateDate <= endDate))
            .ToList();
    }
}