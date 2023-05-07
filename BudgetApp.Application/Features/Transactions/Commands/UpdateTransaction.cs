using BudgetApp.Core.Common;
using BudgetApp.Core.Features.Transactions.Models;
using BudgetApp.Core.Notifications;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.Domain.Interfaces.Repositories;
using MediatR;

namespace BudgetApp.Core.Features.Transactions.Commands;

public class UpdateTransaction : IRequest<ExecutionResult>
{
    public int UserId { get; init; }
    public TransactionModel TransactionModel { get; init; }
}

public class UpdateTransactionHandler : IRequestHandler<UpdateTransaction, ExecutionResult>
{
    private readonly ITransactionRepository transactionRepository;
    private readonly IBudgetRepository budgetRepository;
    private readonly IMediator mediator;

    public UpdateTransactionHandler(ITransactionRepository transactionRepository, IBudgetRepository budgetRepository, IMediator mediator)
    {
        this.transactionRepository = transactionRepository;
        this.budgetRepository = budgetRepository;
        this.mediator = mediator;
    }
    
    public async Task<ExecutionResult> Handle(UpdateTransaction request, CancellationToken cancellationToken)
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
        
        var canPerformAction = await IsUserAuthorizedToPerformActionOnBudget(request.UserId, budget, TransactionActionEnum.Write);
        if (!canPerformAction)
        {
            return new ExecutionResult<bool>(new ErrorInfo(ErrorCode.BudgetError, MessageCode.Unauthorized));
        }
        

        var isBankAccountChanged = transaction.BankAccountId != request.TransactionModel.BankAccountId;
        var isStatusChanged = transaction.Status != request.TransactionModel.Status;
        var isAmountChanged = transaction.Amount != request.TransactionModel.Amount;
        
        if (isBankAccountChanged)
        {
            //Restore amount for original bank account
            if (transaction.Status == TransactionStatusEnum.Completed)
            {
                await PublishAmountChangeNotification(transaction.BankAccountId, transaction.Amount * -1);
            }

            //Apply amount for new bank account
            if (request.TransactionModel.Status == TransactionStatusEnum.Completed)
            {
                await PublishAmountChangeNotification(request.TransactionModel.BankAccountId, request.TransactionModel.Amount);
            }
        } else if (isStatusChanged)
        {
            if (transaction.Status == TransactionStatusEnum.Completed && request.TransactionModel.Status != TransactionStatusEnum.Completed)
            {
                await PublishAmountChangeNotification(transaction.BankAccountId, transaction.Amount * -1);
            }
            else
            {
                await PublishAmountChangeNotification(transaction.BankAccountId, transaction.Amount);
            }
        } else if (isAmountChanged)
        {
            await PublishAmountChangeNotification(transaction.BankAccountId, transaction.Amount * -1);
            await PublishAmountChangeNotification(transaction.BankAccountId, request.TransactionModel.Amount);
        }

        transaction.BankAccountId = request.TransactionModel.BankAccountId;
        transaction.Description = request.TransactionModel.Description;
        transaction.Amount = request.TransactionModel.Amount;
        transaction.Status = request.TransactionModel.Status;
        transaction.UpdateDate = TimeService.Now;

        var update = await transactionRepository.UpdateAsync(transaction);
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

    private async Task PublishAmountChangeNotification(int bankAccountId, decimal amount)
    {
        await mediator.Publish(new TransactionAmountChangedNotification(bankAccountId, amount));
    }
}