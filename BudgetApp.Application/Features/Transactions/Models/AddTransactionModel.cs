using BudgetApp.Domain.Enums;

namespace BudgetApp.Core.Features.Transactions.Models;

public class AddTransactionModel
{
    public int BudgetId { get; set; }

    public decimal Amount { get; set; }

    public TransactionStatusEnum Status { get; set; }

    public string Description { get; set; }
}