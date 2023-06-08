using BudgetApp.Core.Features.BankAccounts.Models;
using BudgetApp.Domain.Enums;

namespace BudgetApp.Core.Features.Transactions.Models;

public class TransactionModel
{
    public int Id { get; init; }
    
    public int BudgetId { get; init; }

    public int BankAccountId { get; set; }

    public BankAccountModel BankAccount { get; set; }

    public decimal Amount { get; set; }

    public string AmountText => $"{Amount} zł";

    public TransactionStatusEnum Status { get; set; }

    public int UserId { get; init; }

    public string Description { get; set; }
    
    public DateTime PaymentDate { get; set; }

    public DateTime CreateDate { get; set; }
    
    public DateTime UpdateDate { get; set; }

    public string ImportHash { get; set; }

    public bool IsHashDuplicated { get; set; }

    public bool ShouldBeAdded { get; set; } = true;
}