using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces.Repositories;
using Dommel;

namespace BudgetApp.Domain.Repositories;

public class BankAccountRepository : BaseRepository<BankAccountEntity>, IBankAccountRepository
{
    public BankAccountRepository(AppSettings appSettings) : base(appSettings)
    {
        
    }
    
    public async Task<List<BankAccountEntity>> GetForBudget(int budgetId)
    {
        using var con = CreateConnection();
        return (await con.SelectAsync<BankAccountEntity>(a => a.BudgetId == budgetId)).ToList();
    }
}