namespace BudgetApp.Core.Features.Transactions.Models;

public class ImportedTransactionScheme
{
    public int PaymentDateIndex { get; set; }

    public int DescriptionIndex { get; set; }

    public int AmountIndex { get; set; }

    public int BankAccountId { get; set; }

}