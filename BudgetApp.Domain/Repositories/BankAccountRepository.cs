using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Interfaces.Repositories;
using Dapper;
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

    public async Task UpdateAccountAmount(int bankAccountId, decimal amount)
    {
        using var con = CreateConnection();
        await con.ExecuteAsync(@$"update bank_accounts set amount = amount + ({amount}) where id = {bankAccountId}");
    }
}