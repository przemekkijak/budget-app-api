using BudgetApp.Application.Common.Models;
using BudgetApp.Application.Common.Services;
using BudgetApp.Core.Features.Transactions.Models;
using BudgetApp.Core.Notifications;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.Domain.Repositories.Interfaces;
using MediatR;

namespace BudgetApp.Core.Features.Transactions.Commands;

public class CreateTransactionCommand : IRequest<ExecutionResult>
{
    public int UserId { get; init; }
    public TransactionModel TransactionModel { get; init; }
}

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, ExecutionResult>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMediator _mediator;
    private readonly IBankAccountRepository _bankAccountRepository;

    public CreateTransactionCommandHandler(IBudgetRepository budgetRepository, ITransactionRepository transactionRepository, IMediator mediator,
        IBankAccountRepository bankAccountRepository)
    {
        _budgetRepository = budgetRepository;
        _transactionRepository = transactionRepository;
        _mediator = mediator;
        _bankAccountRepository = bankAccountRepository;
    }
    
    public async Task<ExecutionResult> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
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
            return await _mediator.Send(new UpdateTransactionCommand()
            {
                TransactionModel = request.TransactionModel,
                UserId = request.UserId
            }, cancellationToken);
        }

        var bankAccount = await _bankAccountRepository.GetByIdAsync(request.TransactionModel.BankAccountId);
        if (bankAccount is null)
        {
            return new ExecutionResult(new ErrorInfo(ErrorCode.BankAccountError, MessageCode.BankAccountNotFound));
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

        transactionEntity.PaymentDate = TimeService.ConvertToUtcTime(transactionEntity.PaymentDate);

        await _transactionRepository.CreateAsync(transactionEntity);

        if (transactionEntity.Status == TransactionStatusEnum.Completed)
        {
                    
            await _mediator.Publish(
                new TransactionAmountChangedNotification(transactionEntity.BankAccountId, transactionEntity.Amount), cancellationToken);
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