using BudgetApp.Core.Notifications;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.BankAccounts.Events;

public class BankAccountAmountUpdated : INotificationHandler<TransactionAmountChangedNotification>
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public BankAccountAmountUpdated(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }
    
    public async Task Handle(TransactionAmountChangedNotification notification, CancellationToken cancellationToken)
    {
        var bankAccount = await _bankAccountRepository.GetByIdAsync(notification.BankAccountId);
        if (bankAccount is null)
        {
            //TODO log critical
            return;
        }

        bankAccount.Amount += notification.Amount;
        await _bankAccountRepository.UpdateAsync(bankAccount);
        //TODO log info about update
    }
}