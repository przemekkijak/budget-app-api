using BudgetApp.Core.Interfaces.Services;
using BudgetApp.Domain;
using BudgetApp.Domain.Common;
using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using BudgetApp.Domain.Interfaces.Repositories;
using BudgetApp.Domain.Models;

namespace BudgetApp.Core;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository transactionRepository;
    private readonly IBudgetRepository budgetRepository;

    public TransactionService(ITransactionRepository transactionRepository, IBudgetRepository budgetRepository)
    {
        this.transactionRepository = transactionRepository;
        this.budgetRepository = budgetRepository;
    }

    public async Task<ExecutionResult> AddTransaction(int userId, AddTransactionModel model)
    {
        if (model.Amount < decimal.Zero)
        {
            return new ExecutionResult(new ErrorInfo(ErrorCode.TransactionError, MessageCode.InvalidAmount));
        }
        
        var budget = await budgetRepository.GetByIdAsync(model.BudgetId);
        if (budget is null)
        {
            return new ExecutionResult(new ErrorInfo(ErrorCode.BudgetError, MessageCode.BudgetNotFound));
        }

        var canPerformAction =
            await IsUserAuthorizedToPerformActionOnBudget(userId, budget, TransactionActionEnum.Write);

        if (!canPerformAction)
        {
            return new ExecutionResult(new ErrorInfo(ErrorCode.BudgetError, MessageCode.Unauthorized));
        }

        var transactionEntity = new TransactionEntity()
        {
            BudgetId = model.BudgetId,
            Amount = model.Amount,
            UserId = userId,
            StatusEnum = model.StatusEnum,
            CreateDate = TimeService.Now,
            UpdateDate = TimeService.Now
        };

        await transactionRepository.CreateAsync(transactionEntity);
        return new ExecutionResult();
    }

    public async Task<ExecutionResult<bool>> UpdateTransaction(int userId, int transactionId, AddTransactionModel model)
    {
        var transaction = await transactionRepository.GetByIdAsync(transactionId);
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
            await IsUserAuthorizedToPerformActionOnBudget(userId, budget, TransactionActionEnum.Write);

        if (!canPerformAction)
        {
            return new ExecutionResult<bool>(new ErrorInfo(ErrorCode.BudgetError, MessageCode.Unauthorized));
        }

        transaction.Amount = model.Amount;
        transaction.StatusEnum = model.StatusEnum;
        transaction.UpdateDate = TimeService.Now;

        var update = await transactionRepository.UpdateAsync(transaction);
        //TODO what if false? Handle and log error 
        return new ExecutionResult<bool>(update);
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