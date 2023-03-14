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

    public async Task<List<TransactionModel>> GetTransactionsForBudget(int budgetId, bool currentMonthOnly = false)
    {
        DateTime? startDate = null;
        DateTime? endDate = null;

        if (currentMonthOnly)
        {
            var now = TimeService.Now;
            startDate = new DateTime(now.Year, now.Month, 1);
            endDate = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
        }

        var entities = await transactionRepository.GetForBudget(budgetId, startDate, endDate);
        return entities.Select(ModelFactory.Create).ToList();
    }

    public async Task<ExecutionResult> AddTransaction(int userId, TransactionModel model)
    {
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
            Status = model.Status,
            Description = model.Description,
            CreateDate = TimeService.Now,
            UpdateDate = TimeService.Now,
            IsDeleted = false
        };

        await transactionRepository.CreateAsync(transactionEntity);
        return new ExecutionResult();
    }

    public async Task<ExecutionResult> UpdateTransaction(int userId, TransactionModel model)
    {
        var transaction = await transactionRepository.GetByIdAsync(model.Id);
        if (transaction is null)
        {
            return await AddTransaction(userId, model);
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
        transaction.Status = model.Status;
        transaction.UpdateDate = TimeService.Now;

        var update = await transactionRepository.UpdateAsync(transaction);
        //TODO what if false? Handle and log error 
        return new ExecutionResult<bool>(update);
    }

    public async Task<ExecutionResult> DeleteTransaction(int userId, int transactionId)
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