using BudgetApp.Core.Common;
using BudgetApp.Core.Features.Transactions.Models;
using BudgetApp.Core.Notifications;
using BudgetApp.Core.Services;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.Transactions.Commands;

public class CreateTransaction : IRequest<ExecutionResult>
{
    public int UserId { get; init; }
    public TransactionModel TransactionModel { get; init; }
}

public class CreateTransactionHandler : IRequestHandler<CreateTransaction, ExecutionResult>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMediator _mediator;

    public CreateTransactionHandler(IBudgetRepository budgetRepository, ITransactionRepository transactionRepository, IMediator mediator)
    {
        _budgetRepository = budgetRepository;
        _transactionRepository = transactionRepository;
        _mediator = mediator;
    }
    
    public async Task<ExecutionResult> Handle(CreateTransaction request, CancellationToken cancellationToken)
    {
        var budget = await _budgetRepository.GetByIdAsync(request.TransactionModel.BudgetId);
        if (budget is null)
        {
            return new ExecutionResult(new ErrorInfo(ErrorCode.BudgetError, MessageCode.BudgetNotFound));
        }

        var canPerformAction = await IsUserAuthorizedToPerformActionOnBudget(request.UserId, budget, TransactionActionEnum.Write);
        if (!canPerformAction)
        {
            return new ExecutionResult(new ErrorInfo(ErrorCode.BudgetError, MessageCode.Unauthorized));
        }

        var existing = await _transactionRepository.GetByIdAsync(request.TransactionModel.Id);
        if (existing is not null)
        {
            return await _mediator.Send(new UpdateTransaction()
            {
                TransactionModel = request.TransactionModel,
                UserId = request.UserId
            }, cancellationToken);
        }

        var transactionEntity = new TransactionEntity
        {
            BudgetId = request.TransactionModel.BudgetId,
            Amount = request.TransactionModel.Amount,
            UserId = request.UserId,
            Status = request.TransactionModel.Status,
            Recipient = request.TransactionModel.Recipient,
            Description = request.TransactionModel.Description,
            PaymentDate = request.TransactionModel.PaymentDate,
            CreateDate = TimeService.Now,
            UpdateDate = TimeService.Now,
            IsDeleted = false,
            BankAccountId = request.TransactionModel.BankAccountId,
            ImportHash = request.TransactionModel.ImportHash
        };

        await _transactionRepository.CreateAsync(transactionEntity);

        await _mediator.Publish(
            new TransactionAmountChangedNotification(transactionEntity.BankAccountId, transactionEntity.Amount), cancellationToken);
        
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