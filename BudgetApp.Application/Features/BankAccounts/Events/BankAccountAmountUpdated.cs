using BudgetApp.Core.Notifications;
using BudgetApp.Domain.Interfaces.Repositories;
using MediatR;

namespace BudgetApp.Core.Features.BankAccounts.Events;

public class BankAccountAmountUpdated : INotificationHandler<TransactionAmountChangedNotification>
{
    private readonly IBankAccountRepository bankAccountRepository;

    public BankAccountAmountUpdated(IBankAccountRepository bankAccountRepository)
    {
        this.bankAccountRepository = bankAccountRepository;
    }
    
    public async Task Handle(TransactionAmountChangedNotification notification, CancellationToken cancellationToken)
    {
        var bankAccount = await bankAccountRepository.GetByIdAsync(notification.BankAccountId);
        if (bankAccount is null)
        {
            //TODO log critical
            return;
        }

        bankAccount.Amount += notification.Amount;
        await bankAccountRepository.UpdateAsync(bankAccount);
        //TODO log info about update
    }
}