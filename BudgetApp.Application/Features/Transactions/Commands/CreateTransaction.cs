using BudgetApp.Core.Common;
using BudgetApp.Core.Features.Transactions.Models;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.Domain.Interfaces.Repositories;
using MediatR;

namespace BudgetApp.Core.Features.Transactions.Commands;

public class CreateTransaction : IRequest<ExecutionResult>
{
    public int UserId { get; init; }
    public TransactionModel TransactionModel { get; init; }
}

public class CreateTransactionHandler : IRequestHandler<CreateTransaction, ExecutionResult>
{
    private readonly IBudgetRepository budgetRepository;
    private readonly ITransactionRepository transactionRepository;

    public CreateTransactionHandler(IBudgetRepository budgetRepository, ITransactionRepository transactionRepository)
    {
        this.budgetRepository = budgetRepository;
        this.transactionRepository = transactionRepository;
    }
    
    public async Task<ExecutionResult> Handle(CreateTransaction request, CancellationToken cancellationToken)
    {
        var budget = await budgetRepository.GetByIdAsync(request.TransactionModel.BudgetId);
        if (budget is null)
        {
            return new ExecutionResult(new ErrorInfo(ErrorCode.BudgetError, MessageCode.BudgetNotFound));
        }

        var canPerformAction = await IsUserAuthorizedToPerformActionOnBudget(request.UserId, budget, TransactionActionEnum.Write);

        if (!canPerformAction)
        {
            return new ExecutionResult(new ErrorInfo(ErrorCode.BudgetError, MessageCode.Unauthorized));
        }

        var transactionEntity = new TransactionEntity
        {
            BudgetId = request.TransactionModel.BudgetId,
            Amount = request.TransactionModel.Amount,
            UserId = request.UserId,
            Status = request.TransactionModel.Status,
            Description = request.TransactionModel.Description,
            CreateDate = TimeService.Now,
            UpdateDate = TimeService.Now,
            IsDeleted = false,
            BankAccountId = request.TransactionModel.BankAccountId
        };

        await transactionRepository.CreateAsync(transactionEntity);
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