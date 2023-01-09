using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces.Repositories;

namespace BudgetApp.Domain.Repositories;

public class TransactionRepository : BaseRepository<TransactionEntity>, ITransactionRepository
{
    protected TransactionRepository(AppSettings appSettings) : base(appSettings)
    {
    }
}