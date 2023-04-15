using BudgetApp.Domain.Enums;

namespace BudgetApp.Domain.Models;

public class TransactionModel
{
    public int Id { get; init; }
    
    public int BudgetId { get; init; }

    public int BankAccountId { get; set; }

    public decimal Amount { get; set; }

    public string AmountText => $"{Amount} zł";

    public TransactionStatusEnum Status { get; set; }

    public int UserId { get; init; }

    public string Description { get; set; }

    //TODO add to entity 
    public DateTime PaymentDate { get; set; }

    public DateTime CreateDate { get; set; }
    
    public DateTime UpdateDate { get; set; }
}