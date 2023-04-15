using BudgetApp.Domain.Models;

namespace BudgetApp.Core.Interfaces.Services;

public interface IBankAccountService
{
    Task<List<BankAccountModel>> GetForBudget(int budgetId);
    Task UpdateAccountAmount(int bankAccountId, decimal amount);
}