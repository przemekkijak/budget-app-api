namespace BudgetApp.Core.Features.ImportTransactions.Models;

public class ImportTransactionSchemeModel
{
    public int UserId { get; set; }

    public string Name { get; set; }
    
    public int PaymentDateIndex { get; set; }

    public int RecipientIndex { get; set; }

    public int DescriptionIndex { get; set; }

    public int AmountIndex { get; set; }

    public int BankAccountId { get; set; }

    public bool SaveTransactionScheme { get; set; }
}