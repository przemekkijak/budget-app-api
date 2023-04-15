namespace BudgetApp.Domain.Models;

public class BudgetModel
{
    public int Id { get; init; }
    
    public string Name { get; init; }

    public int UserId { get; set; }

    public bool IsDefault { get; set; }

    public List<TransactionModel> Transactions { get; set; } = new();

    public List<BankAccountModel> BankAccounts { get; set; } = new();
}