namespace BudgetApp.Core.Features.Transactions.Helpers;

public static class TransactionHelper
{
    public static string GetAmountCellStyle(decimal amount)
    {
        return amount > 0
            ? "green-font"
            : "red-font";
    }
}