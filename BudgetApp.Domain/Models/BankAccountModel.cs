namespace BudgetApp.Domain.Models;

public class BankAccountModel
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public int UserId { get; init; }
    
    public int BudgetId { get; init; }

    public int NumberSuffix { get; set; }

    public bool IsDefault { get; set; }

    public decimal Amount { get; set; }
}