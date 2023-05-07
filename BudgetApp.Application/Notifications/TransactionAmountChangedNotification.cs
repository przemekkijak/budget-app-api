using MediatR;

namespace BudgetApp.Core.Notifications;

public record TransactionAmountChangedNotification(int BankAccountId, decimal Amount) : INotification
{
}