using BudgetApp.Domain;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Repositories.Interfaces;
using Dommel;

namespace BudgetApp.Infrastructure.Repositories;

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

    public async Task<HashSet<string>> GetHashListForBudget(int budgetId)
    {
        using var con = CreateConnection();
        return (await con
                .SelectAsync<TransactionEntity>(a => a.BudgetId == budgetId && a.ImportHash != null))
            .Select(a => a.ImportHash)
            .ToHashSet()!;
    }
}