using BudgetApp.Core.Common;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.Domain.Interfaces.Repositories;
using MediatR;

namespace BudgetApp.Core.Features.Transactions.Commands;

public class DeleteTransactionCommand : IRequest<ExecutionResult>
{
    public int UserId { get; init; }
    public int TransactionId { get; init; }
}

public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, ExecutionResult>
{
    private readonly ITransactionRepository transactionRepository;
    private readonly IBudgetRepository budgetRepository;

    public DeleteTransactionCommandHandler(ITransactionRepository transactionRepository, IBudgetRepository budgetRepository)
    {
        this.transactionRepository = transactionRepository;
        this.budgetRepository = budgetRepository;
    }

    public async Task<ExecutionResult> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
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