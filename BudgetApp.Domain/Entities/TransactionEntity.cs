namespace BudgetApp.Domain.Entities;

public class TransactionEntity : EntityBase
{
    public int BudgetId { get; set; }

    public decimal Amount { get; set; }

    public TransactionStatus Status { get; set; }
    
}