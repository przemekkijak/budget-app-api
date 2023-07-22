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
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly IMediator _mediator;

    public DeleteTransactionHandler(
        ITransactionRepository transactionRepository, 
        IBudgetRepository budgetRepository,
        IMediator mediator)
    {
        _transactionRepository = transactionRepository;
        _budgetRepository = budgetRepository;
        _mediator = mediator;
    }

    public async Task<ExecutionResult> Handle(DeleteTransaction request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId);
        if (transaction is null)
        {
            return new ExecutionResult<bool>(new ErrorInfo(ErrorCode.TransactionError, MessageCode.TransactionNotFound));
        }
        
        var budget = await _budgetRepository.GetByIdAsync(transaction.BudgetId);
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
        await _transactionRepository.UpdateAsync(transaction);

        if (transaction.Status == TransactionStatusEnum.Completed)
        {
            await _mediator.Publish(new TransactionAmountChangedNotification(transaction.BankAccountId, transaction.Amount * -1), cancellationToken);
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