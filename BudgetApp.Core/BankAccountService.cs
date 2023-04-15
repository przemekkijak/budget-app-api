using BudgetApp.Core.Interfaces.Services;
using BudgetApp.Domain.Interfaces.Repositories;
using BudgetApp.Domain.Models;

namespace BudgetApp.Core;

public class BankAccountService : IBankAccountService
{
    private readonly IBankAccountRepository bankAccountRepository;

    public BankAccountService(IBankAccountRepository bankAccountRepository)
    {
        this.bankAccountRepository = bankAccountRepository;
    }

    public async Task<List<BankAccountModel>> GetForBudget(int budgetId)
    {
        var accountEntities = await bankAccountRepository.GetForBudget(budgetId);
        return accountEntities.Select(ModelFactory.Create).ToList();
    }
}