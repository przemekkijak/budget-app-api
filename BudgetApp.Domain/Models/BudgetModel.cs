namespace BudgetApp.Domain.Models;

public class BudgetModel
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public int UserId { get; set; }

    public bool IsDefault { get; set; }

    public decimal Amount { get; set; }

    public List<TransactionModel>? Transactions { get; set; }
}