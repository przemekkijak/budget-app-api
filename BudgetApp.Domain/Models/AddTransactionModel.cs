using BudgetApp.Domain.Enums;
using BudgetApp.Domain.Objects;

namespace BudgetApp.Domain.Models;

public class AddTransactionModel
{
    public int BudgetId { get; set; }

    public decimal Amount { get; set; }

    public TransactionStatusEnum StatusEnum { get; set; }

    public string Description { get; set; }
}