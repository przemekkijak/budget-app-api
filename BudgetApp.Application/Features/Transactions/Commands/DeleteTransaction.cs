using BudgetApp.Core.Common;
using BudgetApp.Core.Notifications;
using BudgetApp.Core.Services;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.Transactions.Commands;

public class DeleteTransaction : IRequest<ExecutionResult>
{
    public int UserId { get; init; }
    public int TransactionId { get; init; }
}

public class DeleteTransactionHandler : IRequestHandler<DeleteTransaction, ExecutionResult>
{
    private readonly ITransactionRepository transactionRepository;
    private readonly IBudgetRepository budgetRepository;
    private readonly IMediator mediator;

    public DeleteTransactionHandler(
        ITransactionRepository transactionRepository, 
        IBudgetRepository budgetRepository,
        IMediator mediator)
    {
        this.transactionRepository = transactionRepository;
        this.budgetRepository = budgetRepository;
        this.mediator = mediator;
    }

    public async Task<ExecutionResult> Handle(DeleteTransaction request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(request.TransactionId);
        if (transaction is null)
        {
            return new ExecutionResult<bool>(new ErrorInfo(ErrorCode.TransactionError, MessageCode.TransactionNotFound));
        }
        
        var budget = await budgetRepository.GetByIdAsync(transaction.BudgetId);
        if (budget is null)
        {
            //TODO Log critical - create logging service 
            return new ExecutionResult<bool>(new ErrorInfo(ErrorCode.BudgetError, MessageCode.BudgetNotFound));
        }
        
        var canPerformAction =
            await IsUserAuthorizedToPerformActionOnBudget(request.UserId, budget, TransactionActionEnum.Write);
        
        if (!canPerformAction)
        {
            return new ExecutionResult<bool>(new ErrorInfo(ErrorCode.BudgetError, MessageCode.Unauthorized));
        }

        transaction.IsDeleted = true;
        transaction.UpdateDate = TimeService.Now;
        await transactionRepository.UpdateAsync(transaction);

        if (transaction.Status == TransactionStatusEnum.Completed)
        {
            await mediator.Publish(new TransactionAmountChangedNotification(transaction.BankAccountId, transaction.Amount * -1), cancellationToken);
        }
        
        return new ExecutionResult();
    }
    
    private async Task<bool> IsUserAuthorizedToPerformActionOnBudget(int userId, BudgetEntity budget,
        TransactionActionEnum action)
    {
        if (userId == budget.UserId)
            return true;

        return false;
        //TODO check budget permissions 
    }
    
}