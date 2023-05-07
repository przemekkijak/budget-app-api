using BudgetApp.Core.Features.BankAccounts.Models;
using BudgetApp.Core.Features.Transactions.Models;

namespace BudgetApp.Core.Features.Budgets.Models;

public class BudgetModel
{
    public int Id { get; init; }
    
    public string Name { get; init; }

    public int UserId { get; set; }

    public bool IsDefault { get; set; }
}