using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces.Repositories;
using Dommel;

namespace BudgetApp.Domain.Repositories;

public class TransactionRepository : BaseRepository<TransactionEntity>, ITransactionRepository
{
    public TransactionRepository(AppSettings appSettings) : base(appSettings)
    {
    }

    public async Task<List<TransactionEntity>> GetForBudget(int budgetId)
    {
        using var con = CreateConnection();
        return (await con.SelectAsync<TransactionEntity>(a => a.BudgetId == budgetId)).ToList();
    }
}