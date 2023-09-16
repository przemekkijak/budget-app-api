namespace BudgetApp.Core.Features.Budgets.Models;

public class BudgetSummary
{
    public decimal CurrentBalance { get; set; }
    
    public decimal IncomingTransactions { get; set; }

    public decimal EndOfTheMonthBalance { get; set; }
}