using BudgetApp.Core.Common;
using BudgetApp.Core.Features.Transactions.Models;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.Domain.Interfaces.Repositories;
using MediatR;

namespace BudgetApp.Core.Features.Transactions.Commands;

public class UpdateTransactionCommand : IRequest<ExecutionResult>
{
    public int UserId { get; init; }
    public TransactionModel TransactionModel { get; init; }
}

public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, ExecutionResult>
{
    private readonly ITransactionRepository transactionRepository;
    private readonly IBudgetRepository budgetRepository;

    public UpdateTransactionCommandHandler(ITransactionRepository transactionRepository, IBudgetRepository budgetRepository)
    {
        this.transactionRepository = transactionRepository;
        this.budgetRepository = budgetRepository;
    }
    
    public async Task<ExecutionResult> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(request.TransactionModel.Id);
        if (transaction is null)
        {
            //TODO return error
            return new ExecutionResult();
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

        //We need to restore account amount, as transaction is not completed anymore
        if (transaction.Status == TransactionStatusEnum.Completed && request.TransactionModel.Status != TransactionStatusEnum.Completed)
        {
            var reversedAmount = transaction.Amount * -1;
            // await bankAccountService.UpdateAccountAmount(transaction.BankAccountId, reversedAmount);
        }

        transaction.Amount = request.TransactionModel.Amount;
        transaction.Status = request.TransactionModel.Status;
        transaction.BankAccountId = request.TransactionModel.BankAccountId;
        transaction.UpdateDate = TimeService.Now;

        
        var update = await transactionRepository.UpdateAsync(transaction);
        
        if (transaction.Status == TransactionStatusEnum.Completed)
        {
            // await bankAccountService.UpdateAccountAmount(transaction.BankAccountId, transaction.Amount);
        }
        //TODO what if false? Handle and log error, those two operations should be done in transaction (for later) 
        
        return new ExecutionResult<bool>(update);
    }
    
    //TODO przeniesc to do helpera
    private async Task<bool> IsUserAuthorizedToPerformActionOnBudget(int userId, BudgetEntity budget,
        TransactionActionEnum action)
    {
        if (userId == budget.UserId)
            return true;

        return false;
        //TODO check budget permissions 
    }
}