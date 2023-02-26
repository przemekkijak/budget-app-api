using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Models;

public class TransactionModel
{
    public int Id { get; set; }
    
    public int BudgetId { get; init; }

    public decimal Amount { get; set; }

    public TransactionStatusEnum Status { get; set; }

    public int UserId { get; init; }

    public string Description { get; set; }

    public DateTime CreateDate { get; set; }
    
    public DateTime UpdateDate { get; set; }
}