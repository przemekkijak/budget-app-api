using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Entities;

public class TransactionEntity : EntityBase
{
    public int BudgetId { get; init; }

    public int BankAccountId { get; set; }

    public BankAccountEntity BankAccount { get; set; }

    public string Recipient { get; set; }

    public decimal Amount { get; set; }

    public TransactionStatusEnum Status { get; set; }

    public int UserId { get; set; }
    
    public string Description { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? ImportHash { get; set; }
}